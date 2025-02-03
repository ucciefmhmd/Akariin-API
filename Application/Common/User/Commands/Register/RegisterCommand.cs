using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Application.Services.UserService;
using Application.Utilities.Extensions;
using Application.Utilities.Models;
using Domain.Common;
using Domain.Identity;
using Infrastructure;
using System.ComponentModel.DataAnnotations;
using Domain.Common.Constants;

namespace Application.Common.User.Commands.Register
{
    public record RegisterCommandResult : BaseCommandResult
    {
    }

    public record RegisterCommand : IRequest<RegisterCommandResult>
    {
        public string? FirstName { get; set; }
        public string? LastName { get; set; }

        [Required(ErrorMessage = nameof(ErrorCode.FieldRequired), AllowEmptyStrings = false)]
        [DataType(DataType.Password)]
        public string Password { get; set; }

        [Required(ErrorMessage = nameof(ErrorCode.FieldRequired), AllowEmptyStrings = false)]
        [EmailAddress(ErrorMessage = nameof(ErrorCode.InvalidEmailAddress))]
        public string Email { get; set; }

        public string? PhoneNumber { get; set; }

        public string? Role { get; set; } = Domain.Common.Constants.Roles.USER;

        public bool IsActive { get; set; }

    }

    [AllowAnonymous]
    public sealed class RegisterCommandHandler(ApplicationDbContext _dbContext, IMediator mediator, UserManager<ApplicationUser> _userManager, IUserService _userService) : IRequestHandler<RegisterCommand, RegisterCommandResult>
    {
        public async Task<RegisterCommandResult> Handle(RegisterCommand request, CancellationToken cancellationToken)
        {            
            if (await _userManager.Users.AnyAsync(x => x.Email == request.Email))
            {
                return new RegisterCommandResult { ErrorCode = ErrorCode.DuplicateEmail };
            }

            if (await _userManager.Users.AnyAsync(x => !string.IsNullOrEmpty(request.PhoneNumber) && x.PhoneNumber == request.PhoneNumber))
            {
                return new RegisterCommandResult { ErrorCode = ErrorCode.DuplicatePhoneNumber };
            }

            var passwordErrors = await _userManager.ValidatePasswordAsync(request.Password);

            if (passwordErrors.Any())
            {
                return new RegisterCommandResult { ErrorCode = ErrorCode.InvalidPasswordRequirements, Errors = passwordErrors };
            }

            var user = new ApplicationUser
            {
                Email = request.Email,
                UserName = request.Email,
                FirstName = request.FirstName??"",
                LastName = request.LastName??request.Email,
                PhoneNumber = request.PhoneNumber,
                PhoneNumberConfirmed = true,
                EmailConfirmed = true,
                CreatedDate = DateTime.UtcNow,
                ModifiedDate = DateTime.UtcNow,
            };

            using (var trans = await _dbContext.Database.BeginTransactionAsync())
            {
                try
                {
                    var result = await _userManager.CreateAsync(user, request.Password);

                    if (!result.Succeeded)
                    {
                        await trans.RollbackAsync();
                        return new RegisterCommandResult { IsSuccess = false, ErrorCode = ErrorCode.Error, Errors = result.Errors.Select(x => x.Description).ToList() };
                    }

                    if (request.Role.Any())
                    {
                        var addUserToRoleResult = await _userService.AssignUserToRolesAsync(user.Id, request.Role);
                       
                        if (!addUserToRoleResult.IsSuccess)
                        {
                            await trans.RollbackAsync();
                            return new RegisterCommandResult
                            {
                                ErrorCode = ErrorCode.Error,
                                IsSuccess = false,
                                Errors = addUserToRoleResult.Errors
                            };
                        }
                    }

                    //var response = await mediator.Send(new SendConfirmEmail.SendConfirmEmailCommand { Email = user.Email });

                    await trans.CommitAsync();

                    //var loginResponse = await mediator.Send(new LoginCommand { UserName = user.Email , Password = request.Password });

                    return new RegisterCommandResult
                    {
                        IsSuccess = true
                        //Token = loginResponse.IsSuccess ? loginResponse.Token : null
                    };



                }
                catch (Exception ex)
                {
                    await trans.RollbackAsync();
                    return new RegisterCommandResult
                    {
                        ErrorCode = ErrorCode.Error,
                        IsSuccess = false,
                        Errors = { ex.Message }
                    };
                }
            }
        }



    }
}
