using MediatR;
using Microsoft.Extensions.Logging;
using Infrastructure;
using Domain.Identity;
using Application.Utilities.Models;
using Application.Utilities.Extensions;
using Application.Utilities.Sort;
using Application.Utilities.Filter;
using Microsoft.AspNetCore.Identity;
using Application.Services.File;

namespace Application.Common.User.Queries.GetUsers
{
    public record GetUsersQueryResult : BaseCommandResult
    {
        public BasePaginatedList<UserVM> Result { get; set; }
    }
    public record GetUsersQuery : BasePaginatedQuery, IRequest<GetUsersQueryResult>
    {

    }
    public record UserVM
    {
        public string Id { get; set; }
        public string Name { get; set; }
        public string Email { get; set; }
        public string PhoneNumber { get; set; }
        public bool IsActive { get; set; }
        public string Role { get; set; }
        public string Image { get; set; }
        public DateTime CreatedDate { get; set; }
        public DateTime ModifiedDate { get; set; }
    }
    //public sealed class GetUsersHandler : IRequestHandler<GetUsersQuery, GetUsersQueryResult>
    //{
    //    private readonly ApplicationDbContext _dbContext;
    //    private readonly ILogger<GetUsersHandler> _logger;
    //    private readonly UserManager<ApplicationUser> _userManager;
    //    private readonly AttachmentService _attachmentService;

    //    public GetUsersHandler(ApplicationDbContext dbContext, ILogger<GetUsersHandler> logger,UserManager<ApplicationUser> userManager, AttachmentService attachmentService)
    //    {
    //        _dbContext = dbContext;
    //        _logger = logger;
    //        _userManager = userManager;
    //        _attachmentService = attachmentService;
    //    }

    //    public async Task<GetUsersQueryResult> Handle(GetUsersQuery request, CancellationToken cancellationToken)
    //    {
    //        try
    //        {
    //            var result = await _dbContext.Users
    //                .Search(request.SearchTerm)
    //                .Select(u => new UserVM
    //                {
    //                    Email = u.Email,
    //                    Id = u.Id,
    //                    Name = u.Name,
    //                    PhoneNumber = u.PhoneNumber ?? "",
    //                    IsActive = u.LockoutEnd == null ? true : false,
    //                    Image = u.Image,
    //                    Role = u.Role
    //                })
    //                .Filter(request.Filters)
    //                .Sort(request.Sorts ?? new List<SortedQuery>() { new SortedQuery() { PropertyName = "FirstName", Direction = SortDirection.ASC } })
    //                .ToPaginatedListAsync(request.PageNumber, request.PageSize);

    //            foreach (var item in result.Items)
    //            {
    //                var user = await _userManager.FindByIdAsync(item.Id);

    //                var roles = await _userManager.GetRolesAsync(user);

    //                item.Role = roles.FirstOrDefault();
    //            }

    //            foreach (var user in result.Items)
    //            {
    //                var profile = await _attachmentService.GetFilesUrlAsync(Path.Combine("profiles", user.Id.ToString()));

    //                if (profile.IsSuccess && profile.Urls.Count > 0)
    //                {
    //                    user.Image = profile.Urls[0];
    //                }
    //            }
    //            if (result == null)
    //            {
    //                return new GetUsersQueryResult
    //                {
    //                    ErrorCode = Domain.Common.ErrorCode.UserNotFound,
    //                    IsSuccess = false,
    //                    Result = null
    //                };
    //            }
    //            return new GetUsersQueryResult
    //            {
    //                IsSuccess = true,
    //                Result = result
    //            };
    //        }
    //        catch (Exception ex)
    //        {
    //            _logger.LogError(ex.Message, ex);
    //            return new GetUsersQueryResult
    //            {
    //                ErrorCode = Domain.Common.ErrorCode.Error,
    //                IsSuccess = false,
    //                Result = null
    //            };
    //        }
    //    }
    //}


    public sealed class GetUsersHandler : IRequestHandler<GetUsersQuery, GetUsersQueryResult>
    {
        private readonly ApplicationDbContext _dbContext;
        private readonly ILogger<GetUsersHandler> _logger;
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly AttachmentService _attachmentService;

        public GetUsersHandler(ApplicationDbContext dbContext, ILogger<GetUsersHandler> logger, UserManager<ApplicationUser> userManager, AttachmentService attachmentService)
        {
            _dbContext = dbContext;
            _logger = logger;
            _userManager = userManager;
            _attachmentService = attachmentService;
        }

        public async Task<GetUsersQueryResult> Handle(GetUsersQuery request, CancellationToken cancellationToken)
        {
            try
            {
                // Step 1: Fetch all users with their basic details
                var usersQuery = _dbContext.Users
                    .Search(request.SearchTerm)
                    .Select(u => new UserVM
                    {
                        Email = u.Email,
                        Id = u.Id,
                        Name = u.Name,
                        PhoneNumber = u.PhoneNumber ?? "",
                        IsActive = u.LockoutEnd == null ? true : false,
                        Image = u.Image,
                        CreatedDate = u.CreatedDate,
                        ModifiedDate = u.ModifiedDate,
                        Role = u.Role // This will be updated later
                    });

                // Step 2: Handle role filtering separately
                var roleFilter = request.Filters?.FirstOrDefault(f => f.PropertyName == "Role");
                if (roleFilter != null)
                {
                    var roleValue = roleFilter.Values.FirstOrDefault();
                    if (!string.IsNullOrEmpty(roleValue))
                    {
                        // Fetch users with the specified role
                        var userIdsInRole = (await _userManager.GetUsersInRoleAsync(roleValue))
                            .Select(u => u.Id)
                            .ToList();

                        // Filter the users based on the role
                        usersQuery = usersQuery.Where(u => userIdsInRole.Contains(u.Id));
                    }
                }

                // Step 3: Apply other filters (excluding the Role filter)
                var otherFilters = request.Filters?.Where(f => f.PropertyName != "Role").ToList();
                if (otherFilters != null && otherFilters.Any())
                {
                    usersQuery = usersQuery.Filter(otherFilters);
                }

                // Step 4: Apply sorting and pagination
                var result = await usersQuery
                    .Sort(request.Sorts ?? new List<SortedQuery>() { new SortedQuery() { PropertyName = "FirstName", Direction = SortDirection.ASC } })
                    .ToPaginatedListAsync(request.PageNumber, request.PageSize);

                // Step 5: Fetch roles for each user and update the Role property
                foreach (var item in result.Items)
                {
                    var user = await _userManager.FindByIdAsync(item.Id);
                    var roles = await _userManager.GetRolesAsync(user);
                    item.Role = roles.FirstOrDefault();
                }

                // Step 6: Fetch profile images for each user
                foreach (var user in result.Items)
                {
                    var profile = await _attachmentService.GetFilesUrlAsync(Path.Combine("profiles", user.Id.ToString()));
                    if (profile.IsSuccess && profile.Urls.Count > 0)
                    {
                        user.Image = profile.Urls[0];
                    }
                }

                // Step 7: Return the result
                if (result == null)
                {
                    return new GetUsersQueryResult
                    {
                        ErrorCode = Domain.Common.ErrorCode.UserNotFound,
                        IsSuccess = false,
                        Result = null
                    };
                }

                return new GetUsersQueryResult
                {
                    IsSuccess = true,
                    Result = result
                };
            }
            catch (Exception ex)
            {
                _logger.LogError(ex.Message, ex);
                return new GetUsersQueryResult
                {
                    ErrorCode = Domain.Common.ErrorCode.Error,
                    IsSuccess = false,
                    Result = null
                };
            }
        }
    }
}
