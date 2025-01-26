using Application.Common.User.Commands.UpdateUserSocials;
using Application.Services.File;
using Application.Services.UserService;
using Application.Utilities.Models;
using Domain.Common;
using Infrastructure;
using MediatR;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Common.User.Queries.GetUserProfile
{
    public record GetUserSocialQueryResult : BaseCommandResult
    {
        public UserSocialDataVM User { get; set; }
    }
    public record GetUserSocialQuery : IRequest<GetUserSocialQueryResult>
    {
        
    }
 
    public class GetUserSocialQueryHandler : IRequestHandler<GetUserSocialQuery, GetUserSocialQueryResult>
    {
        private readonly IUserService _userService;

        public GetUserSocialQueryHandler(IUserService userService)
        {
            this._userService = userService;
        }
        public async Task<GetUserSocialQueryResult> Handle(GetUserSocialQuery request, CancellationToken cancellationToken)
        {
            try
            {
                var user = await _userService.GetCurrentUserAsync();
                if (user == null)
                {
                    return new GetUserSocialQueryResult
                    {
                        ErrorCode = ErrorCode.AccessDenied,
                        Errors = { "Please Login First" },
                        IsSuccess = false
                    };
                }
                return new GetUserSocialQueryResult
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
                return new GetUserSocialQueryResult
                {
                    ErrorCode = ErrorCode.Error,
                    Errors = { ex.Message },
                    IsSuccess = false
                };
            }

        }
    }
}
