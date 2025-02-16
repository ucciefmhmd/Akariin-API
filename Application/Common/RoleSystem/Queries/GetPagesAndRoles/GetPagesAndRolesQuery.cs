using Application.Common.User.Queries.GetUsers;
using Application.Utilities.Models;
using Infrastructure;
using MediatR;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Common.RoleSystem.Queries.GetPagesAndRoles
{

    public record GetPagesAndRolesQueryResult : BaseCommandResult
    {
        public List<PagesAndRolesVM> PagesAndRoles { get; set; }
    }
    public record GetPagesAndRolesQuery : IRequest<GetPagesAndRolesQueryResult>
    {
        public string UserId { get; set; }
       
    }
    public record PagesAndRolesVM
    {
        public List<UserRoleVM> Roles { get; set; } = new List<UserRoleVM>();
        public Guid PageId { get; set; }
        public string PageName { get; set; }
        public string PagePath { get; set; }
    }
    public record UserRoleVM
    {
        public string Id { get; set; }
        public string Name { get; set; }
    }
    public class GetPagesAndRolesQueryHandler : IRequestHandler<GetPagesAndRolesQuery, GetPagesAndRolesQueryResult>
    {
        private readonly ApplicationDbContext _context;

        public GetPagesAndRolesQueryHandler(ApplicationDbContext context)
        {
            this._context = context;
        }
        public async Task<GetPagesAndRolesQueryResult> Handle(GetPagesAndRolesQuery request, CancellationToken cancellationToken)
        {
            try
            {
              

                var user = await _context.Users.FirstOrDefaultAsync(p => p.Id == request.UserId);
                if (user == null)
                {
                    return new GetPagesAndRolesQueryResult
                    {
                        ErrorCode = Domain.Common.ErrorCode.NotFound,
                        Errors = { "User Not Found" },
                        IsSuccess = false
                    };
                }
               var pagesAndRoles = await _context.UserPageRoles
                    .Include(u => u.Role)
                    .Include(u => u.Page)
                    .Where(upr => upr.UserId == request.UserId)
                    .GroupBy(upr => new { upr.PageId, upr.Page.Name ,upr.Page.Path})
                    .Select(g => new PagesAndRolesVM
                    {
                        PageId = g.Key.PageId,
                        PageName = g.Key.Name,
                        PagePath = g.Key.Path,
                        Roles = g.Select(r =>  new UserRoleVM { Id = r.RoleId , Name = r.Role.Name }).ToList()
                    })
                    .ToListAsync();

                pagesAndRoles.Insert(0,new PagesAndRolesVM { PageId = Guid.Empty, PageName = "dashboard", PagePath = "/dashboard" });
                return new GetPagesAndRolesQueryResult
                {
                    IsSuccess = true,
                    PagesAndRoles = pagesAndRoles
                };
            }
            catch (Exception ex)
            {
                return new GetPagesAndRolesQueryResult
                {
                    ErrorCode = Domain.Common.ErrorCode.Error,
                    Errors = { ex.Message },
                    IsSuccess = false
                };
            }
        }
    }
}
