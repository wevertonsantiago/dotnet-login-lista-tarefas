using System.Text.Json.Serialization;
using configs;
using dataContext;
using Microsoft.AspNetCore.Mvc.ApplicationModels;
using Microsoft.EntityFrameworkCore;
using middleware;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddControllers(options =>
{
    options.Conventions.Add(new RouteTokenTransformerConvention(
        new EndpointLowercaseLetter()));
});

builder.Services.AddEndpointsApiExplorer();

builder.Services.AddDbContext<DataContext>(opt =>
{
    opt.UseSqlite(builder.Configuration.GetConnectionString("DefaultConnection"));
});

builder.AddCorsConfigs();
builder.AddSwaggerConfigs();
builder.AddAuthConfigs();
builder.AddRateLimiterConfigs();
builder.AddServicesConfigs();
builder.AddEnumStringSwaggerConfigs();

var app = builder.Build();

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseMiddleware<UnauthorizedMiddleware>();

app.UseHttpsRedirection();

app.UseCors("CorsPolicy");

app.UseAuthentication();

app.UseAuthorization();

app.UseRateLimiter();

app.MapControllers();

app.Run();
