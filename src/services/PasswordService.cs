using Microsoft.AspNetCore.Identity;

namespace services;
public class PasswordService
{
    readonly PasswordHasher<string> passwordHasher = new PasswordHasher<string>();
    public string CreatePassword(string password)
    {
        return passwordHasher.HashPassword(null!, password);
    }

    public bool VerifyPassword(string passwordSaved, string password)
    {
        var result = passwordHasher.VerifyHashedPassword(null!, passwordSaved, password);

        if (result == PasswordVerificationResult.Success)
        {
            return true;
        }
        else
        {
            return false;
        }
    }
}

