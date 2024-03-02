using System.Security.Claims;

namespace services;
public class UserIdService
{
    public string GetUserId(ClaimsPrincipal user)
    {
        var userId = user.FindFirstValue(ClaimTypes.NameIdentifier);

        if (string.IsNullOrEmpty(userId))
        {
            throw new Exception("Acesso negado!");
        }

        return userId;
    }
}

