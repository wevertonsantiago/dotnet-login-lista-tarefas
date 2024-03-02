using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Security.Cryptography;
using System.Text;
using dataContext;
using entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;

namespace services;

public class AuthService
{
    public JwtSecurityToken GererateAccessToken(IEnumerable<Claim> claims, IConfiguration _configuration)
    {
        var secretKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_configuration["JWT:SecretKey"]!));

        JwtSecurityToken token = new JwtSecurityToken(
            issuer: _configuration["JWT:ValidAudience"],
            audience: _configuration["JWT:ValidIssuer"],
            claims: claims,
            notBefore: DateTime.Now, // Definir NotBefore como o tempo atual em UTC
            expires: DateTime.Now.AddHours(double.Parse(_configuration["JWT:ExpireHours"]!)),
            signingCredentials: new SigningCredentials(secretKey, SecurityAlgorithms.HmacSha256)
        );

        return token;
    }

    public string GenerateRefreshToken()
    {
        var secureRandomBytes = new byte[128];
        using var randomNumberGenerator = RandomNumberGenerator.Create();

        randomNumberGenerator.GetBytes(secureRandomBytes);
        var refreshtoken = Convert.ToBase64String(secureRandomBytes);
        return refreshtoken;
    }

    public ClaimsPrincipal GetPrincipalFromExpiredToken(string token, IConfiguration _configuration)
    {
        var tokenValidationParameters = new TokenValidationParameters
        {
            ValidateAudience = false,
            ValidateIssuer = false,
            ValidateIssuerSigningKey = true,
            IssuerSigningKey = new SymmetricSecurityKey(
            Encoding.UTF8.GetBytes(_configuration["JWT:SecretKey"]!)),
            ValidateLifetime = false
        };
        var tokenHandler = new JwtSecurityTokenHandler();
        try
        {
            var principal = tokenHandler.ValidateToken(token, tokenValidationParameters, out SecurityToken securityToken);


            if (securityToken is not JwtSecurityToken jwtSecurityToken ||
                !jwtSecurityToken.Header.Alg.Equals(
                SecurityAlgorithms.HmacSha256,
                StringComparison.InvariantCultureIgnoreCase))
            {
                throw new SecurityTokenException();
            }
            return principal;
        }
        catch (SecurityTokenException)
        {
            // Captura a exceção e retorna uma mensagem de erro genérica
            throw new SecurityTokenException("Invalid token");
        }
    }

    public async Task<UserEntity?> GetUserByEmailAsync(DataContext _context, string Email)
    {
        return await _context.Users.FirstOrDefaultAsync(u => u.Email == Email);
    }
}