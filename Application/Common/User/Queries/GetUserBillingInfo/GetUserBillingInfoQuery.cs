using Application.Common.User.Commands.UpdateUserBillingInfo;
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

namespace Application.Common.User.Queries.GetUserBillingInfo
{

    public record GetUserBillingInfoQueryResult : BaseCommandResult
    {
        public UserBillingInfoVM User { get; set; }
    }
    public record GetUserBillingInfoQuery : IRequest<GetUserBillingInfoQueryResult>
    {
        public string? Id { get; set; }
    }
    public class GetUserBillingInfoQueryHandler : IRequestHandler<GetUserBillingInfoQuery, GetUserBillingInfoQueryResult>
    {
        private readonly ApplicationDbContext _context;
        private readonly IUserService _userService;

        public GetUserBillingInfoQueryHandler(ApplicationDbContext context, IUserService userService)
        {
            _context = context;
            this._userService = userService;
        }
        public async Task<GetUserBillingInfoQueryResult> Handle(GetUserBillingInfoQuery request, CancellationToken cancellationToken)
        {
            try
            {
                var currentUser = await _userService.GetCurrentUserAsync();
                var user = await _context.Users.FirstOrDefaultAsync(u => u.Id == request.Id);
                if (user == null)
                {
                    if (currentUser == null)
                    {
                        return new GetUserBillingInfoQueryResult
                        {
                            ErrorCode = ErrorCode.UserNotFound,
                            Errors = { "User Not Found" },
                            IsSuccess = false
                        };
                    }
                    user = currentUser;
                }

                var resultUser = new UserBillingInfoVM
                {
                    BillingFirstName = user.BillingFirstName,
                    BillingMainPhoneNumber = user.BillingMainPhoneNumber,
                    BillingFullNameInEnglish = user.BillingFullNameInEnglish,
                    BillingFamilyName = user.BillingFamilyName,
                    BillingAlternativePhoneNumber = user.BillingAlternativePhoneNumber,
                    BillingFatherName = user.BillingFatherName,
                    Id = user.Id
                };

                return new GetUserBillingInfoQueryResult
                {
                    IsSuccess = true,
                    User = resultUser
                };
            }
            catch (Exception ex)
            {
                return new GetUserBillingInfoQueryResult
                {
                    ErrorCode = ErrorCode.Error,
                    Errors = { ex.Message },
                    IsSuccess = false
                };
            }

        }
    }
}
