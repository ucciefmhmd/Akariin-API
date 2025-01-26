using Application.Services.File;
using Application.Utilities.Extensions;
using Application.Utilities.Filter;
using Application.Utilities.Models;
using Application.Utilities.Sort;
using Domain.Identity;
using Infrastructure;
using MediatR;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Common.User.Queries.GetUsersForProfessional
{
 
    public record GetUsersForProfessionalQueryResult : BaseCommandResult
    {
        public BasePaginatedList<UserProVM> Result { get; set; }
    }
    public record GetUsersForProfessionalQuery : BasePaginatedQuery, IRequest<GetUsersForProfessionalQueryResult>
    {
        public string? Role { get; set; }
    }
    public record UserProVM
    {
        public string Id { get; set; }
        public string Name { get; set; }
        public string Email { get; set; }
        public string Image { get; set; }
        public bool IsActive { get; set; }
    }
    public sealed class GetUsersHandler : IRequestHandler<GetUsersForProfessionalQuery, GetUsersForProfessionalQueryResult>
    {
        private readonly ApplicationDbContext _dbContext;
        private readonly AttachmentService _attachmentService;
        private readonly ILogger<GetUsersHandler> _logger;

        public GetUsersHandler(ApplicationDbContext dbContext, ILogger<GetUsersHandler> logger, LocalFiletService localFiletService)
        {
            _dbContext = dbContext;
            _logger = logger;
            _attachmentService = new AttachmentService(localFiletService);
        }

        public async Task<GetUsersForProfessionalQueryResult> Handle(GetUsersForProfessionalQuery request, CancellationToken cancellationToken)
        {
            try
            {
                var resultQuery =  _dbContext.Users
                    .Search(request.SearchTerm);

                if(request.Role != null)
                {
                    var role = await _dbContext.Roles.FirstOrDefaultAsync(r => r.Name == request.Role);
                    if(role != null)
                    {
                        var usersInRoleIds = await _dbContext.UserRoles.Where(ur => ur.RoleId ==  role.Id).Select(ur => ur.UserId).ToListAsync();
                        resultQuery = resultQuery.Where(u => usersInRoleIds.Contains(u.Id));
                    }
                }
                var result = await resultQuery
                    .Select(u => new UserProVM
                    {
                        Email = u.Email,
                        Id = u.Id,
                        Name = u.Name,
                        Image = "",
                        IsActive = u.IsActive
                    })
                    .Filter(request.Filters)
                    .Sort(request.Sorts ?? new List<SortedQuery>() { new SortedQuery() { PropertyName = "FirstName", Direction = SortDirection.ASC } })
                    .ToPaginatedListAsync(request.PageNumber, request.PageSize);

                foreach (var user in result.Items)
                {
                    var profile = await _attachmentService.GetFilesUrlAsync(Path.Combine("profiles", user.Id));
                    if (profile.IsSuccess)
                    {
                        user.Image = profile.Urls.Count > 0 ? profile.Urls[0] : "";
                    }
                }
               
                return new GetUsersForProfessionalQueryResult
                {
                    IsSuccess = true,
                    Result = result
                };
            }
            catch (Exception ex)
            {
                _logger.LogError(ex.Message, ex);
                return new GetUsersForProfessionalQueryResult
                {
                    ErrorCode = Domain.Common.ErrorCode.Error,
                    IsSuccess = false,
                };
            }
        }
    }
}
