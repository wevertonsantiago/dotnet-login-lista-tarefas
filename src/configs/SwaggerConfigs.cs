using Microsoft.OpenApi.Models;

namespace configs;

public static class SwaggerConfigs
{
    public static void AddSwaggerConfigs(this WebApplicationBuilder builder)
    {
        builder.Services.AddSwaggerGen(swagger =>
        {
            swagger.SwaggerDoc("v1", new OpenApiInfo { Title = "Login Tasks", Version = "v1" });

            swagger.AddSecurityDefinition("Bearer", new OpenApiSecurityScheme()
            {
                Name = "Authorization",
                Type = SecuritySchemeType.ApiKey,
                Scheme = "Bearer",
                BearerFormat = "JWT",
                In = ParameterLocation.Header,
                Description = "Value = Bearer + token",
            });

            swagger.AddSecurityRequirement(new OpenApiSecurityRequirement
            {
                {
                    new OpenApiSecurityScheme
                    {
                        Reference = new OpenApiReference
                        {
                            Type = ReferenceType.SecurityScheme,
                            Id = "Bearer"
                        }
                    },
                    new string[] {}
                }
            });
        });

    }
}