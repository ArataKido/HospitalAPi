using Microsoft.AspNetCore.Authorization;

namespace accounts;

public static class AuthorizationPolicies
{
    public static void AddAuthorizationPolicies(this IServiceCollection services)
    {
        services.AddAuthorization(options =>
        {
            options.AddPolicy("AdminRole", policy => policy.RequireRole("admin"));
            options.AddPolicy("DoctorRole", policy => policy.RequireRole("doctor"));
            options.AddPolicy("PatientRole", policy => policy.RequireRole("patient"));
            options.AddPolicy("UserRole", policy => policy.RequireRole("user"));
        });
    }
}