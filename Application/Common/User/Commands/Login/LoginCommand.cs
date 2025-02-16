using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using Domain.Common;
using Domain.Identity;
using System.ComponentModel.DataAnnotations;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using Application.Utilities.Models;
using Microsoft.Extensions.Configuration;

namespace Application.Common.User.Commands.Login
{
    public record LoginCommand : IRequest<LoginCommandResult>
    {
        [Required]
        public string UserName { get; set; }

        [Required]
        [DataType(DataType.Password)]
        public string Password { get; set; }
    }

    public record LoginCommandResult : BaseCommandResult
    {
        public bool IsLockout { get; set; }
        public bool IsNotAllowed { get; set; }
        public bool IsActive { get; set; }
        public string Token { get; set; } = string.Empty;
    }

   
    [AllowAnonymous]
    public sealed class LoginCommandHandler(IConfiguration _configuration, SignInManager<ApplicationUser> _signInManager) : IRequestHandler<LoginCommand, LoginCommandResult>
    {
        public async Task<LoginCommandResult> Handle(LoginCommand request, CancellationToken cancellationToken)
        {
            try
            {
                var model = new LoginCommandResult();

                var User = await _signInManager.UserManager.Users.FirstOrDefaultAsync(x => x.Email!.ToLower().Trim().Equals(request.UserName.ToLower().Trim())
                                                                                   || x.UserName.Trim().ToLower().Equals(request.UserName.Trim().ToLower())
                                                                                   || x.PhoneNumber.Trim().Equals(request.UserName.Trim().ToLower()));

                if (User == null) return new LoginCommandResult() { IsSuccess = false, ErrorCode = ErrorCode.UserNotFound };
               
                var result = await _signInManager.CheckPasswordSignInAsync(User, request.Password, lockoutOnFailure: true);

                if (result.Succeeded)
                {
                    await _signInManager.UserManager.UpdateAsync(User);

                    var token = await CreateJwtToken(User);

                    model.IsSuccess = true;

                    model.IsActive = User.IsActive;

                    model.Token = new JwtSecurityTokenHandler().WriteToken(token);

                    return model;
                }
                
                if (result.IsLockedOut)
                {
                    return new LoginCommandResult() { IsSuccess = false, IsLockout = true };
                }
                if (result.IsNotAllowed)
                {
                    return new LoginCommandResult() { IsSuccess = false, IsNotAllowed = true };
                }
                else
                {
                    return new LoginCommandResult() { IsSuccess = false, ErrorCode = ErrorCode.InvalidLogin };
                }

            }
            catch (Exception ex)
            {
                return new LoginCommandResult() { IsSuccess = false, ErrorCode = ErrorCode.Error };
            }
        }

        private async Task<JwtSecurityToken> CreateJwtToken(ApplicationUser user)
        {
            var userClaims = await GetUserClaims(user);

            var roles = await GetUserRoles(user);

            var roleClaims = new List<Claim>();

            foreach (var role in roles)
            {
                roleClaims.Add(new Claim("roles", role));
            }

            var claims = new[]
            {
                new Claim(JwtRegisteredClaimNames.Sub, user.UserName),
                new Claim(JwtRegisteredClaimNames.Jti, user.Id),
                new Claim(JwtRegisteredClaimNames.Email, user.Email),
                new Claim("uss",user.SecurityStamp)
            }.Union(userClaims).Union(roleClaims);

            foreach (var claim in claims)
            {
                Console.WriteLine($"Claim Type: {claim.Type}, Claim Value: {claim.Value}");
            }

            var symmetricSecurityKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_configuration["JWT:Key"]));

            var signingCredentials = new SigningCredentials(symmetricSecurityKey, SecurityAlgorithms.HmacSha256);

            var jwtSecurityToken = new JwtSecurityToken(issuer: _configuration["JWT:Issuer"],
                                                        audience: _configuration["JWT:Audience"],
                                                        claims: claims,
                                                        expires: DateTime.UtcNow.AddDays(1),
                                                        signingCredentials: signingCredentials);

            return jwtSecurityToken;
        }


        #region Helper Methods

        async Task<List<Claim>> GetUserClaims(ApplicationUser user)
        {
            var result = await _signInManager.UserManager.GetClaimsAsync(user);
            return result.ToList();
        }
        async Task<List<string>> GetUserRoles(ApplicationUser user)
        {
            var result = await _signInManager.UserManager.GetRolesAsync(user);
            return result.ToList();
        }

        # endregion


    }


}
