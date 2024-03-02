using System.Threading.RateLimiting;
using Microsoft.AspNetCore.RateLimiting;

namespace configs;

public static class RateLimiterConfigs
{
    public static void AddRateLimiterConfigs(this WebApplicationBuilder builder)
    {
        builder.Services.AddRateLimiter(rateOptions =>
        {
            rateOptions.AddFixedWindowLimiter(policyName: "rateLimitRquest", options =>
            {
                options.PermitLimit = 1;
                options.Window = TimeSpan.FromSeconds(5);
                options.QueueLimit = 2;
                options.QueueProcessingOrder = QueueProcessingOrder.OldestFirst;
            });
            rateOptions.RejectionStatusCode = StatusCodes.Status429TooManyRequests;
        });

    }
}