using dto;
using Microsoft.AspNetCore.Mvc;

namespace exceptionError;

public static class InternalServerErrorException
{
    public static ActionResult InternalServerError(this ControllerBase controller, Exception ex)
    {
        controller.HttpContext.RequestServices.GetService<ILogger<ControllerBase>>()?.LogError(ex, "Internal server error");
        return controller.StatusCode(500, new ResponseDTO { Status = "Error", Message = $"Internal server error: {ex.Message}" });
    }
}
