using Application.Utilities.Models;
using Domain.Identity;

namespace Application.Services.UserService
{
    public interface IUserService
    {
        public  Task<string> GetSecurityStamp(string Id);
        
        public Task<string> GenerateResetCodeAsync(string userId);
        
        public Task<bool> VerifyResetCodeAsync(string userId, string code);

        public Task<BaseCommandResult> AssignUserToRolesAsync(string userId, string RoleName);

        public Task<ApplicationUser> GetCurrentUserAsync();
        
        public Task<bool> CheckAdminPermissionAsync();

    }
}
