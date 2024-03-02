using System.Text.Json;
using dto;

namespace middleware;
public class UnauthorizedMiddleware
{
    private readonly RequestDelegate _next;

    public UnauthorizedMiddleware(RequestDelegate next)
    {
        _next = next;
    }

    public async Task Invoke(HttpContext context)
    {
        await _next(context);

        if (context.Response.StatusCode == StatusCodes.Status401Unauthorized)
        {
            var response = new ResponseDTO
            {
                Status = "Error",
                Message = "Sem autorização."
            };
            var jsonResponse = JsonSerializer.Serialize(response);

            context.Response.ContentType = "application/json";

            await context.Response.WriteAsync(jsonResponse);

        }
        if (context.Response.StatusCode == StatusCodes.Status429TooManyRequests)
        {
            var response = new ResponseDTO
            {
                Status = "Error",
                Message = "Muitas requisições ao mesmo tempo, aguarde alguns segundos."
            };
            var jsonResponse = JsonSerializer.Serialize(response);

            context.Response.ContentType = "application/json";

            await context.Response.WriteAsync(jsonResponse);
        }
    }
}