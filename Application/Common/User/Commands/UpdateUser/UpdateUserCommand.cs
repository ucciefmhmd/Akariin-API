using MediatR;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using Application.Services.UserService;
using Application.Utilities.Models;
using Domain.Common;
using Domain.Common.Attributes;
using Domain.Identity;
using Infrastructure;
using System.ComponentModel.DataAnnotations;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using Domain.Common.Constants;
using Application.Services.File;

namespace Application.Common.User.Commands.UpdateUser
{
    public record UpdateUserCommandResult : BaseCommandResult{}
    public record UpdateUserCommand : IRequest<UpdateUserCommandResult>
    {
        [Required]
        public string Id { get; set; }
        [Trim]
        public string? Email { get; set; }

        [Required(ErrorMessage = nameof(ErrorCode.FieldRequired), AllowEmptyStrings = false)]
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string? PhoneNumber { get; set; }
        public IFormFile? Image { get; set; }
        public string? Role { get; set; }
        public bool IsActive { get; set; }
    }

    public sealed class UpdateUserCommandHandler : IRequestHandler<UpdateUserCommand, UpdateUserCommandResult>
    {
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly ApplicationDbContext _dbContext;
        private readonly AttachmentService _attachmentService;
        private readonly IUserService _userService;
        private readonly IHttpContextAccessor _httpContext;
        private readonly ILogger<UpdateUserCommandHandler> _logger;

        public UpdateUserCommandHandler(UserManager<ApplicationUser> userManager,
            ApplicationDbContext dbContext,
            IUserService userService,
            IHttpContextAccessor httpContext,
            LocalFiletService localFiletService
            , ILogger<UpdateUserCommandHandler> logger)
        {
            _userManager = userManager;
            _dbContext = dbContext;
            _userService = userService;
            _httpContext = httpContext;
            _logger = logger;
            _attachmentService = new AttachmentService(localFiletService);
        }
        public async Task<UpdateUserCommandResult> Handle(UpdateUserCommand request, CancellationToken cancellationToken)
        {
            var userId = request.Id ?? _httpContext.HttpContext.User.FindFirstValue(JwtRegisteredClaimNames.Jti);

            var user = await _userManager.FindByIdAsync(userId);

            if (user == null)
            {
                return new UpdateUserCommandResult()
                {
                    ErrorCode = ErrorCode.UserNotFound,
                    IsSuccess = false
                };
            }
            var isSystemAdmin = await  _userManager.IsInRoleAsync(user ,Roles.ADMIN) && user.UserName.Equals(Roles.ADMIN, StringComparison.OrdinalIgnoreCase);
            //if ()
            //{
            //    return new UpdateUserCommandResult()
            //    {
            //        ErrorCode = ErrorCode.AccessDenied,
            //        IsSuccess = false,
            //        Errors = { "Can not modify Admin" }
            //    };
            //}
            using (var trans = await _dbContext.Database.BeginTransactionAsync(cancellationToken))
            {
                try
                {
                    
                    if (!string.IsNullOrEmpty(request.Email))
                    {
                        if (await _userManager.Users.FirstOrDefaultAsync(x => x.Email.Trim().ToLower() == request.Email.Trim().ToLower() && userId != x.Id) != null)
                        {
                            await trans.RollbackAsync(cancellationToken);
                            return new UpdateUserCommandResult()
                            {
                                ErrorCode = ErrorCode.DuplicateEmail,
                                IsSuccess = false
                            };
                        }
                        user.UserName = user.Email = request.Email;
                    }
                    if (!string.IsNullOrEmpty(request.PhoneNumber) && await _userManager.Users.FirstOrDefaultAsync(x => x.PhoneNumber.Trim() == request.PhoneNumber.Trim() && userId != x.Id) != null)
                    {
                        await trans.RollbackAsync(cancellationToken);
                        return new UpdateUserCommandResult()
                        {
                            ErrorCode = ErrorCode.DuplicatePhoneNumber,
                            IsSuccess = false
                        };
                    }
                    
                    user.PhoneNumber = request.PhoneNumber;
                    user.FirstName = request.FirstName;
                    user.LastName = request.LastName;
                    user.ModifiedDate = DateTime.UtcNow;
                    user.IsActive = request.IsActive;
                    
                    var result = await _userManager.UpdateAsync(user);
                    
                    if (!result.Succeeded)
                    {
                        await trans.RollbackAsync(cancellationToken);
                        return new UpdateUserCommandResult
                        {
                            ErrorCode = ErrorCode.Error,
                            IsSuccess = false,
                            Errors = result.Errors.Select(x => x.Description).ToList()
                        };
                    }
                   
                    if (request.Role != null)
                    {
                        if (isSystemAdmin && request.Role != Roles.ADMIN)
                        {
                            request.Role = Roles.ADMIN;
                        }
                        var addUserToRoleResult = await _userService.AssignUserToRolesAsync(user.Id, request.Role);
                        if (!addUserToRoleResult.IsSuccess)
                        {
                            await trans.RollbackAsync();
                            return new UpdateUserCommandResult
                            {
                                ErrorCode = ErrorCode.Error,
                                IsSuccess = false,
                                Errors = addUserToRoleResult.Errors
                            };
                        }
                    }
                    if(request.Image != null)
                    {
                        await _attachmentService.UploadFilesAsync(Path.Combine("profiles",request.Id),request.Image);
                    }

                    await trans.CommitAsync(cancellationToken);
                    return new UpdateUserCommandResult()
                    {
                        IsSuccess = true
                    };
                }
                catch (Exception ex)
                {
                    await trans.RollbackAsync(cancellationToken);
                    _logger.LogError(ex.Message, ex);
                    return new UpdateUserCommandResult()
                    {
                        ErrorCode = ErrorCode.Error,
                        Errors = { ex.Message }
                    };
                }
            }
        }
    }
}
