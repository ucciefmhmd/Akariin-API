using Application.Services.File;
using Application.Utilities.Models;
using Domain.Common;
using Domain.Common.Attributes;
using Infrastructure;
using MediatR;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Common.User.Commands.UpdateUserSocials
{
    public record UpdateUserSocialsCommandResult : BaseCommandResult
    {
        public UserSocialDataVM User { get; set; }
    }
    public record UpdateUserSocialsCommand : IRequest<UpdateUserSocialsCommandResult>
    {
        [Required]
        public string Id { get; set; }
        public string? Facebook { get; set; }
        public string? Twitter { get; set; }
        public string? LinkedIn { get; set; }
        public string? WebsiteOrBlog { get; set; }
    }
    public class UserSocialDataVM
    {
        public string Id { get; set; }
        public string? Facebook { get; set; }
        public string? Twitter { get; set; }
        public string? LinkedIn { get; set; }
        public string? WebsiteOrBlog { get; set; }
    }
    public class UpdateUserSocialsCommandHandler : IRequestHandler<UpdateUserSocialsCommand, UpdateUserSocialsCommandResult>
    {
        private readonly ApplicationDbContext _context;

        public UpdateUserSocialsCommandHandler(ApplicationDbContext context, LocalFiletService localFiletService)
        {
            _context = context;
        }
        public async Task<UpdateUserSocialsCommandResult> Handle(UpdateUserSocialsCommand request, CancellationToken cancellationToken)
        {
            try
            {
                var user = await _context.Users.FirstOrDefaultAsync(u => u.Id == request.Id);
                if (user == null)
                {
                    return new UpdateUserSocialsCommandResult
                    {
                        ErrorCode = ErrorCode.UserNotFound,
                        Errors = { "User Not Found" },
                        IsSuccess = false
                    };
                }
                user.Facebook = request.Facebook;
                user.Twitter = request.Twitter;
                user.LinkedIn = request.LinkedIn;
                user.WebsiteOrBlog = request.WebsiteOrBlog;
                _context.Users.Update(user);
                await _context.SaveChangesAsync();
               

                return new UpdateUserSocialsCommandResult
                {
                    IsSuccess = true,
                    User = new UserSocialDataVM
                    {
                        Facebook = user.Facebook,
                        Twitter = user.Twitter,
                        Id = user.Id,
                        LinkedIn = user.LinkedIn,
                        WebsiteOrBlog = user.WebsiteOrBlog
                    }
                };
            }
            catch (Exception ex)
            {
                return new UpdateUserSocialsCommandResult
                {
                    ErrorCode = ErrorCode.Error,
                    Errors = { ex.Message },
                    IsSuccess = false
                };
            }

        }
    }
}
