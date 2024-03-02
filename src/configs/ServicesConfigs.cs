using interfaces;
using repositories;
using services;

namespace configs;

public static class ServicesConfigs
{
    public static void AddServicesConfigs(this WebApplicationBuilder builder)
    {
        builder.Services.AddScoped<AuthService>()
                        .AddScoped<UserIdService>()
                        .AddScoped<PasswordService>()
                        .AddScoped<IUnitOfWork, UnitOfWork>();

    }
}