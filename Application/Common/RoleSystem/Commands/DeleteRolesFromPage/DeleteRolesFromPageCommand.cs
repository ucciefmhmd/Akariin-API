using Application.Utilities.Models;
using Infrastructure;
using MediatR;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Common.RoleSystem.Commands.DeleteRolesFromPage
{
    public record DeleteRolesFromPageCommandResult : BaseCommandResult
    {
    }
    public record DeleteRolesFromPageCommand : IRequest<DeleteRolesFromPageCommandResult>
    {
        public string UserId { get; set; }
        public List<string> Roles { get; set; } = new List<string>();
        public Guid? PageId { get; set; }
    }
    public class DeleteRolesFromPageCommandHandler : IRequestHandler<DeleteRolesFromPageCommand, DeleteRolesFromPageCommandResult>
    {
        private readonly ApplicationDbContext _context;

        public DeleteRolesFromPageCommandHandler(ApplicationDbContext context)
        {
            this._context = context;
        }
        public async Task<DeleteRolesFromPageCommandResult> Handle(DeleteRolesFromPageCommand request, CancellationToken cancellationToken)
        {
            try
            {
                if (request.PageId.HasValue)
                {
                    var page = await _context.Pages.FirstOrDefaultAsync(p => p.Id == request.PageId);
                    if (page == null)
                    {
                        return new DeleteRolesFromPageCommandResult
                        {
                            ErrorCode = Domain.Common.ErrorCode.NotFound,
                            Errors = { "Page Not Found" },
                            IsSuccess = false
                        };
                    }
                }
                

                var user = await _context.Users.FirstOrDefaultAsync(p => p.Id == request.UserId);
                if (user == null)
                {
                    return new DeleteRolesFromPageCommandResult
                    {
                        ErrorCode = Domain.Common.ErrorCode.NotFound,
                        Errors = { "User Not Found" },
                        IsSuccess = false
                    };
                }
                if(request.PageId.HasValue)
                {
                    foreach (var role in request.Roles)
                    {
                        var roleId = await _context.Roles.Where(r => r.Name == role).Select(r => r.Id).FirstOrDefaultAsync();

                        var userPageRole = await _context.UserPageRoles.FirstOrDefaultAsync(u => u.UserId == user.Id && u.RoleId == roleId && u.PageId == request.PageId);
                        if (userPageRole != null)
                        {
                            _context.UserPageRoles.Remove(userPageRole);
                        }
                    }
                }
                else
                {
                    var userPageRoles = await _context.UserPageRoles.Where(u => u.UserId == user.Id).ToListAsync();
                    _context.UserPageRoles.RemoveRange(userPageRoles);
                }

                await _context.SaveChangesAsync();
                return new DeleteRolesFromPageCommandResult
                {
                    IsSuccess = true
                };
            }
            catch (Exception ex)
            {
                return new DeleteRolesFromPageCommandResult
                {
                    ErrorCode = Domain.Common.ErrorCode.Error,
                    Errors = { ex.Message },
                    IsSuccess = false
                };
            }
        }
    }
}
