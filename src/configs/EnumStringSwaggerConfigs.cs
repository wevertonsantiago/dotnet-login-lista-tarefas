using System.Text.Json.Serialization;

namespace configs;

public static class EnumStringSwaggerConfigs
{
    public static void AddEnumStringSwaggerConfigs(this WebApplicationBuilder builder)
    {

        builder.Services.AddControllers()
                        .AddJsonOptions(options =>
                        {
                            options.JsonSerializerOptions.Converters.Add(new JsonStringEnumConverter());
                        });

    }
}