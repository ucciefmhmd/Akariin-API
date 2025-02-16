using Application.Utilities.Models;
using Infrastructure;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace Application.Common.RoleSystem.Queries.GetRolesInPage
{
    public record GetRolesInPageQueryResult : BaseCommandResult
    {
        public List<string> Roles { get; set; }
    }
    public record GetRolesInPageQuery : IRequest<GetRolesInPageQueryResult>
    {
        public string UserId { get; set; }
        public Guid PageId { get; set; }

    }

    public class GetRolesInPageQueryHandler : IRequestHandler<GetRolesInPageQuery, GetRolesInPageQueryResult>
    {
        private readonly ApplicationDbContext _context;

        public GetRolesInPageQueryHandler(ApplicationDbContext context)
        {
            this._context = context;
        }
        public async Task<GetRolesInPageQueryResult> Handle(GetRolesInPageQuery request, CancellationToken cancellationToken)
        {
            try
            {


                var user = await _context.Users.FirstOrDefaultAsync(p => p.Id == request.UserId);
                if (user == null)
                {
                    return new GetRolesInPageQueryResult
                    {
                        ErrorCode = Domain.Common.ErrorCode.NotFound,
                        Errors = { "User Not Found" },
                        IsSuccess = false
                    };
                }
                var roles = await _context.UserPageRoles
                    .Include(u => u.Role)
                     .Where(upr => upr.UserId == request.UserId && upr.PageId == request.PageId)
                     .Select(upr => upr.Role.Name)
                     .ToListAsync();
                return new GetRolesInPageQueryResult
                {
                    IsSuccess = true,
                    Roles = roles
                };
            }
            catch (Exception ex)
            {
                return new GetRolesInPageQueryResult
                {
                    ErrorCode = Domain.Common.ErrorCode.Error,
                    Errors = { ex.Message },
                    IsSuccess = false
                };
            }
        }
    }
}
