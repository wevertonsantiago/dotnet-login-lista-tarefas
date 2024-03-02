using System.Text;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.IdentityModel.Tokens;

namespace configs;

public static class AuthConfigs
{
    public static void AddAuthConfigs(this WebApplicationBuilder builder)
    {
        var secretKey = builder.Configuration["JWT:SecretKey"]
                         ?? throw new ArgumentException("Invalid secret key!!");
        builder.Services.AddAuthentication(options =>
        {
            options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
            options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
        }).AddJwtBearer(options =>
            {
                options.SaveToken = true;
                options.RequireHttpsMetadata = false;
                options.TokenValidationParameters = new TokenValidationParameters()
                {
                    ValidateIssuer = true,
                    ValidateAudience = true,
                    ValidateLifetime = true,
                    ValidateIssuerSigningKey = true,
                    ClockSkew = TimeSpan.Zero,
                    ValidAudience = builder.Configuration["JWT:ValidAudience"],
                    ValidIssuer = builder.Configuration["JWT:ValidIssuer"],
                    IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(secretKey))
                };
            });

        // builder.Services.AddAuthorization(options =>
        // {
        //     options.AddPolicy("CustomPolicy", policy =>
        //     {
        //         policy.RequireAuthenticatedUser().Build();
        //     });
        // });
    }
}