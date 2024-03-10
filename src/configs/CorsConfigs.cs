using interfaces;
using repositories;
using services;

namespace configs;

public static class CorsConfigs
{
    public static void AddCorsConfigs(this WebApplicationBuilder builder)
    {
        builder.Services.AddCors(options =>
      {
          options.AddPolicy("CorsPolicy",
              builder => builder
                    .AllowAnyOrigin()
                    .AllowAnyMethod()
                    .AllowAnyHeader()
                  );
      });

    }
}