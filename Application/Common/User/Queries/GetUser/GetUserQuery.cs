using MediatR;
using Domain.Identity;
using Infrastructure;
using System.ComponentModel.DataAnnotations;
using Microsoft.Extensions.Logging;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Http;
using System.Security.Claims;
using Microsoft.IdentityModel.JsonWebTokens;
using Application.Utilities.Models;
using Application.Services.File;
using Domain.Common.Constants;

namespace Application.Common.User.Queries.GetUser
{

    public record GetUserQueryResult : BaseCommandResult
    {
        public ApplicationUser User { get; set; }

    }
   
    public record GetUserQuery : IRequest<GetUserQueryResult>
    {
        [Required]
        public string? Id { get; set; }
    }
    public sealed class GetUserHandler : IRequestHandler<GetUserQuery, GetUserQueryResult>
    {
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly ApplicationDbContext _dbContext;
        private readonly AttachmentService _attachmentService;
        private readonly ILogger<GetUserHandler> _logger;
        private readonly IHttpContextAccessor _httpContext;

        public GetUserHandler(ApplicationDbContext dbContext,LocalFiletService localFiletService ,UserManager<ApplicationUser> userManager, ILogger<GetUserHandler> logger, IHttpContextAccessor httpContext)
        {
            _dbContext = dbContext;
            _logger = logger;
            _httpContext = httpContext;
            _userManager = userManager;
            _attachmentService = new AttachmentService(localFiletService);
        }

        public async Task<GetUserQueryResult> Handle(GetUserQuery request, CancellationToken cancellationToken)
        {
            try
            {
                var userId = request.Id ?? _httpContext.HttpContext.User.FindFirstValue(JwtRegisteredClaimNames.Jti);

                var user = await _dbContext.Users
                    .FirstOrDefaultAsync(u => u.Id == userId);

                if (user == null)
                {
                    return new GetUserQueryResult
                    {
                        ErrorCode = Domain.Common.ErrorCode.UserNotFound,
                        IsSuccess = false,
                        User = null
                    };
                }
                user.Roles = await _userManager.GetRolesAsync(user);
                var url = "";
                var profile = await _attachmentService.GetFilesUrlAsync(Path.Combine("profiles", user.Id));
                if (profile.IsSuccess)
                {
                    user.Image = profile.Urls.Count > 0 ? profile.Urls[0] : "";
                }
                return new GetUserQueryResult
                {

                    IsSuccess = true,
                    User = user
                };

            }
            catch (Exception ex)
            {
                _logger.LogError(ex.Message, ex);

                return new GetUserQueryResult
                {
                    ErrorCode = Domain.Common.ErrorCode.Error,
                    IsSuccess = false
                };
            }
        }
    }
}
