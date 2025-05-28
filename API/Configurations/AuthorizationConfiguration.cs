using Domain.Common.Constants;

namespace Configurations
{
    public static class AuthorizationConfiguration
    {
        public static IServiceCollection AddAuthorizationConfiguration(this IServiceCollection services)
        {
            services.AddAuthorizationBuilder()
                .AddPolicy(Policies.ADMIN_SUB_ADMIN_POLICY, policy =>
                    policy.RequireRole(Roles.ADMIN, Roles.SUB_ADMIN))
                .AddPolicy(Policies.ADMIN_POLICY, policy =>
                    policy.RequireRole(Roles.ADMIN))
                .AddPolicy(Policies.USER_POLICY, policy =>
                    policy.RequireRole(Roles.ADMIN, Roles.SUB_ADMIN, Roles.USER));

            return services;
        }
    }
}