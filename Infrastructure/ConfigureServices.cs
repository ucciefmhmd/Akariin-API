using Infrastructure;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Domain.Identity;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.IdentityModel.Tokens;
using System.Text;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using Infrastructure.Interceptors;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;

namespace Microsoft.Extensions.DependencyInjection;

public static class ConfigureServices
{
    public static IServiceCollection AddInfrastructureServices(this IServiceCollection services, IConfiguration configuration)
    {
        services.AddDbContext<ApplicationDbContext>(options =>
            options.UseSqlServer(configuration.GetConnectionString("DefaultConnection"),
                sqlOptions => sqlOptions.MigrationsAssembly(typeof(ApplicationDbContext).Assembly.FullName)));

        services.AddScoped<ApplicationDbContextInitialiser>();
        services.AddScoped<AuditableEntitySaveChangesInterceptor>();

        services.AddDefaultIdentity<ApplicationUser>()
            .AddRoles<IdentityRole>()
            .AddRoleManager<RoleManager<IdentityRole>>()
            .AddEntityFrameworkStores<ApplicationDbContext>()
            .AddDefaultTokenProviders();

        services.Configure<IdentityOptions>(options =>
        {
            // Password settings.
            options.Password.RequiredLength = 6;
            options.Password.RequireDigit =
               options.Password.RequireLowercase =
               options.Password.RequireNonAlphanumeric =
               options.Password.RequireUppercase = false;

            // Lockout settings.
            options.Lockout.DefaultLockoutTimeSpan = TimeSpan.FromMinutes(5);
            options.Lockout.MaxFailedAccessAttempts = int.MaxValue;
            options.Lockout.AllowedForNewUsers = true;

            // User settings.
            options.User.RequireUniqueEmail = true;
            options.User.AllowedUserNameCharacters = null;

            // Sign-in settings.
            options.SignIn.RequireConfirmedEmail = true;
            options.SignIn.RequireConfirmedPhoneNumber = false;
        });

        services.Configure<SecurityStampValidatorOptions>(options =>
        {
            // Enables immediate logout after updating the user's security stamp.
            options.ValidationInterval = TimeSpan.Zero;
        });

        services.AddAuthentication(options =>
        {
            options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
            options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
        })
        .AddJwtBearer(options =>
        {
            options.SaveToken = false;
            options.RequireHttpsMetadata = false;
            options.TokenValidationParameters = new TokenValidationParameters()
            {
                ValidateIssuer = true,
                ValidateAudience = true,
                ValidateLifetime = true,
                ValidateIssuerSigningKey = true,
                ValidAudience = configuration["JWT:Audience"],
                ValidIssuer = configuration["JWT:Issuer"],
                IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(configuration["JWT:Key"])),
                ClockSkew = TimeSpan.Zero
            };

            options.Events = new JwtBearerEvents
            {
                OnTokenValidated = async context =>
                {
                    try
                    {
                        var userIdClaim = context.Principal?.FindFirst(JwtRegisteredClaimNames.Jti)?.Value;
                        var securityStampClaim = context.Principal?.FindFirst("uss")?.Value;

                        if (string.IsNullOrEmpty(userIdClaim) || string.IsNullOrEmpty(securityStampClaim))
                        {
                            context.Fail("Invalid token: Missing required claims.");
                            return;
                        }

                        var userManager = context.HttpContext.RequestServices.GetRequiredService<UserManager<ApplicationUser>>();
                        var user = await userManager.FindByIdAsync(userIdClaim);

                        if (user == null || user.SecurityStamp != securityStampClaim)
                        {
                            context.Fail("Security stamp mismatch or user not found.");
                        }
                    }
                    catch (Exception ex)
                    {
                        context.Fail($"Token validation failed: {ex.Message}");
                    }
                },

                OnChallenge = context =>
                {
                    context.HandleResponse();
                    context.Response.StatusCode = 401;
                    context.Response.ContentType = "application/json";
                    var result = System.Text.Json.JsonSerializer.Serialize(new { message = "Unauthorized - Token is invalid or expired" });
                    return context.Response.WriteAsync(result);
                },

                OnMessageReceived = context =>
                {
                    var logger = context.HttpContext.RequestServices.GetRequiredService<ILogger<JwtBearerEvents>>();

                    // Extract token from query string
                    var accessToken = context.Request.Query["token"];

                    // If not found in query, check the Authorization header
                    if (string.IsNullOrEmpty(accessToken))
                    {
                        var authHeader = context.Request.Headers["Authorization"].FirstOrDefault();
                        if (!string.IsNullOrEmpty(authHeader) && authHeader.StartsWith("Bearer "))
                        {
                            accessToken = authHeader.Substring("Bearer ".Length).Trim();
                        }
                    }

                    //// If not found in query, check the Authorization header
                    //if (string.IsNullOrEmpty(accessToken))
                    //{
                    //    var authHeader = context.Request.Headers["Authorization"].FirstOrDefault();

                    //    if (!string.IsNullOrEmpty(authHeader))
                    //    {
                    //        accessToken = authHeader;
                    //    }
                    //}

                    // Log received token
                    if (string.IsNullOrEmpty(accessToken))
                    {
                        logger.LogWarning("❌ No access token received in query string or Authorization header.");
                    }
                    else
                    {
                        logger.LogInformation($"✅ Token received: {accessToken}");
                    }

                    context.Token = accessToken;
                    return Task.CompletedTask;
                }

            };
        });

        services.Configure<DataProtectionTokenProviderOptions>(o => o.TokenLifespan = TimeSpan.FromMinutes(15));

        services.AddAuthorization();

        return services;
    }
}
