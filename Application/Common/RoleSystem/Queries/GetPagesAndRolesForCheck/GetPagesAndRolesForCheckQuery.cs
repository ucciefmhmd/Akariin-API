using Application.Services.File;
using Application.Utilities.Models;
using Infrastructure;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace Application.Common.RoleSystem.Queries.GetPagesAndRolesForCheck
{

    public record GetPagesAndRolesForCheckQueryResult : BaseCommandResult
    {
        public List<AllPagesAndRolesVM> PagesAndRoles { get; set; }
        public CreatedByVM User { get; set; }
    }
    public record GetPagesAndRolesForCheckQuery : IRequest<GetPagesAndRolesForCheckQueryResult>
    {
        public string UserId { get; set; }
    }
    public record AllPagesAndRolesVM
    {
        public List<UserRoleChecksVM> Roles { get; set; } = new List<UserRoleChecksVM>();
        public Guid PageId { get; set; }
        public string PageName { get; set; }
        public string PagePath { get; set; }
    }
    public record CreatedByVM
    {
        public string Id { get; set; }
        public string Name { get; set; }
        public string Image { get; set; }
    }
    public record UserRoleChecksVM
    {
        public string RoleName { get; set; }
        public bool IsGranted { get; set; }
    }
    public record UserDataVM: CreatedByVM
    {
        public string? Email { get; set; }
    }
    public class GetPagesAndRolesForCheckQueryHandler : IRequestHandler<GetPagesAndRolesForCheckQuery, GetPagesAndRolesForCheckQueryResult>
    {
        private readonly ApplicationDbContext _context;
        private readonly AttachmentService _attachmentService;

        public GetPagesAndRolesForCheckQueryHandler(ApplicationDbContext context, LocalFiletService localFiletService)
        {
            this._context = context;
            _attachmentService = new AttachmentService(localFiletService);
        }
        public async Task<GetPagesAndRolesForCheckQueryResult> Handle(GetPagesAndRolesForCheckQuery request, CancellationToken cancellationToken)
        {
            try
            {
                var user = await _context.Users.FirstOrDefaultAsync(p => p.Id == request.UserId);
                if (user == null)
                {
                    return new GetPagesAndRolesForCheckQueryResult
                    {
                        ErrorCode = Domain.Common.ErrorCode.NotFound,
                        Errors = { "User Not Found" },
                        IsSuccess = false
                    };
                }
                var userPagesRoles = await _context.UserPageRoles.Include(upr => upr.Role).Where(upr => upr.UserId == request.UserId).ToListAsync();

                var pages = await _context.Pages.Select(p => new { Name = p.Name, Id = p.Id, Path = p.Path }).ToListAsync(cancellationToken);

                var pagesAndRoles = new List<AllPagesAndRolesVM>();
                foreach (var page in pages)
                {
                    pagesAndRoles.Add(new AllPagesAndRolesVM
                    {
                        PageId = page.Id,
                        PageName = page.Name,
                        PagePath = page.Path,
                        Roles = new List<UserRoleChecksVM>
                         {
                            new UserRoleChecksVM
                            {
                                RoleName = "Create",
                                IsGranted = userPagesRoles.Any(upr => upr.PageId == page.Id && upr.Role.Name == "Create")
                            },
                            new UserRoleChecksVM
                            {
                                RoleName = "Update",
                                IsGranted = userPagesRoles.Any(upr => upr.PageId == page.Id && upr.Role.Name == "Update")
                            },
                            new UserRoleChecksVM
                            {
                                RoleName = "Delete",
                                IsGranted = userPagesRoles.Any(upr => upr.PageId == page.Id && upr.Role.Name == "Delete")
                            },
                            new UserRoleChecksVM
                            {
                                RoleName = "Read",
                                IsGranted = userPagesRoles.Any(upr => upr.PageId == page.Id && upr.Role.Name == "Read")
                            }
                         }
                    });
                }

                var userVM = new UserDataVM();
                var profiles = await _attachmentService.GetFilesUrlAsync(Path.Combine("profiles", request.UserId));
                if (profiles.IsSuccess)
                {
                    userVM.Image = profiles.Urls.Count > 0 ? profiles.Urls[0] : ""; ;
                }
                userVM.Id = request.UserId;
                userVM.Name = user.Name;
                userVM.Email = user.Email;


                return new GetPagesAndRolesForCheckQueryResult
                {
                    IsSuccess = true,
                    PagesAndRoles = pagesAndRoles,
                    User = userVM
                };
            }
            catch (Exception ex)
            {
                return new GetPagesAndRolesForCheckQueryResult
                {
                    ErrorCode = Domain.Common.ErrorCode.Error,
                    Errors = { ex.Message },
                    IsSuccess = false
                };
            }
        }
    }
}
