using Application.Utilities.Models;
using Infrastructure;
using MediatR;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Common.RoleSystem.Commands.AddRolesInPage
{
    public record AddRolesInPageCommandResult : BaseCommandResult
    {
    }
    public record AddRolesInPageCommand:IRequest<AddRolesInPageCommandResult>
    {
        public string UserId { get; set; }
        public List<string> Roles { get; set; } = new List<string>();
        public List<Guid> PageIds { get; set; } = new List<Guid>();
    }
    public class AddRolesInPageCommandHandler(ApplicationDbContext _context) : IRequestHandler<AddRolesInPageCommand, AddRolesInPageCommandResult>
    {
        public async Task<AddRolesInPageCommandResult> Handle(AddRolesInPageCommand request, CancellationToken cancellationToken)
        {
            try
            {
                //var page = await _context.Pages.FirstOrDefaultAsync(p => p.Id == request.PageId);
                //if (page == null)
                //{
                //    return new AddRolesInPageCommandResult
                //    {
                //        ErrorCode = Domain.Common.ErrorCode.NotFound,
                //        Errors = { "Page Not Found" },
                //        IsSuccess = false
                //    };
                //}
                
                var user = await _context.Users.FirstOrDefaultAsync(p => p.Id == request.UserId);
                if (user == null)
                {
                    return new AddRolesInPageCommandResult
                    {
                        ErrorCode = Domain.Common.ErrorCode.NotFound,
                        Errors = { "User Not Found" },
                        IsSuccess = false
                    };
                }
                foreach (var pageId in request.PageIds)
                {
                    var page = await _context.Pages.FirstOrDefaultAsync(p => p.Id == pageId);
                    if (page == null)
                    {
                        return new AddRolesInPageCommandResult
                        {
                            ErrorCode = Domain.Common.ErrorCode.NotFound,
                            Errors = { "Page Not Found" },
                            IsSuccess = false
                        };
                    }
                    foreach (var role in request.Roles)
                    {
                        var roleId = await _context.Roles.Where(r => r.Name == role).Select(r => r.Id).FirstOrDefaultAsync();
                        var userPageRole = await _context.UserPageRoles.FirstOrDefaultAsync(u => u.UserId == user.Id && u.RoleId == roleId && u.PageId == page.Id);
                        if (userPageRole == null)
                        {
                            userPageRole = new Domain.Models.RoleSysem.UserPageRole
                            {
                                Id = Guid.NewGuid(),
                                PageId = page.Id,
                                RoleId = roleId,
                                UserId = user.Id,
                            };
                            await _context.UserPageRoles.AddAsync(userPageRole);
                        }
                    }
                }
               
                await _context.SaveChangesAsync();
                return new AddRolesInPageCommandResult
                {
                    IsSuccess = true
                };
            }
            catch (Exception ex)
            {
                return new AddRolesInPageCommandResult
                {
                    ErrorCode = Domain.Common.ErrorCode.Error,
                    Errors = { ex.Message },
                    IsSuccess = false
                };
            }
        }
    }
}
