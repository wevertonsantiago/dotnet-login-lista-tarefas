using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using dataContext;
using dto;
using entities;
using exceptionError;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.RateLimiting;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using model.UserTokenDto;
using services;

namespace controller;

[ApiController]
[Route("api/[controller]")]
[EnableRateLimiting("rateLimitRquest")]
public class AuthController : ControllerBase
{
    private readonly AuthService _authService;
    private readonly DataContext _context;
    private readonly IConfiguration _configuration;
    private readonly PasswordService _passwordService;

    public AuthController(AuthService authService, DataContext context, IConfiguration configuration, PasswordService passwordService)
    {
        _authService = authService;
        _context = context;
        _configuration = configuration;
        _passwordService = passwordService;
    }

    [HttpPost("register")]
    [ProducesResponseType(typeof(ResponseDTO), StatusCodes.Status201Created)]
    [ProducesResponseType(typeof(ResponseDTO), StatusCodes.Status400BadRequest)]
    [ProducesResponseType(typeof(ResponseDTO), StatusCodes.Status500InternalServerError)]
    public async Task<ActionResult> RegisterUser([FromBody] RegisterDTO model)
    {
        var userByEmail = await _authService.GetUserByEmailAsync(_context, model.Email);

        if (userByEmail != null)
            return Conflict(new ResponseDTO { Status = "Error", Message = "User already exists!" });

        try
        {
            var password = _passwordService.CreatePassword(model.Password);

            UserEntity user = new()
            {
                Id = Guid.NewGuid().ToString(),
                Email = model.Email,
                Password = password,
                DateUpgrade = DateTime.Now,
                DateCreate = DateTime.Now,
            };

            await _context.Users.AddAsync(user);
            await _context.SaveChangesAsync();

            return Created("/Register", new ResponseDTO { Status = "Success", Message = "User created successfully" });

        }
        catch (Exception ex)
        {
            return this.InternalServerError(ex);
        }

    }

    [HttpPost("login")]
    [ProducesResponseType(typeof(UserTokenDto), StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(ResponseDTO), StatusCodes.Status400BadRequest)]
    [ProducesResponseType(typeof(ResponseDTO), StatusCodes.Status500InternalServerError)]
    public async Task<ActionResult> Login([FromBody] LoginDTO model)
    {
        var userByEmail = await _authService.GetUserByEmailAsync(_context, model.Email);
        if (userByEmail == null)
            return BadRequest(new ResponseDTO { Status = "Error", Message = "Invalid email or password!" });

        var verifyPassword = _passwordService.VerifyPassword(userByEmail.Password!, model.Password);
        if (verifyPassword == false)
            return BadRequest(new ResponseDTO { Status = "Error", Message = "Invalid email or password!" });

        if (userByEmail != null && verifyPassword == true)
        {

            var authClaims = new List<Claim>
            {
                new Claim("userID", userByEmail.Id!),
                new Claim(ClaimTypes.Email, userByEmail.Email!),
                new Claim(ClaimTypes.NameIdentifier, userByEmail.Id!),
            };

            var token = _authService.GererateAccessToken(authClaims, _configuration);

            var refreshtoken = _authService.GenerateRefreshToken();

            userByEmail.RefreshToken = refreshtoken;
            userByEmail.RefreshTokenExpirytime = DateTime.Now.AddHours(double.Parse(_configuration["JWT:RefreshTokenValidityInHours"]!));

            _context.Update(userByEmail);
            await _context.SaveChangesAsync();

            return Ok(new
            {
                Token = new JwtSecurityTokenHandler().WriteToken(token),
                Refreshtoken = refreshtoken,
                Expiration = token.ValidTo
            });
        }
        else
        {
            return BadRequest(new ResponseDTO { Status = "Success", Message = "Incorrect email or password" });
        }
    }

    [HttpPost("refresh-token")]
    [ProducesResponseType(typeof(UserTokenDto), StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(ResponseDTO), StatusCodes.Status400BadRequest)]
    [ProducesResponseType(typeof(ResponseDTO), StatusCodes.Status500InternalServerError)]
    public async Task<ActionResult> RefreshTokenAsync([FromHeader] string expiredToken)
    {
        if (expiredToken is null)
            return BadRequest(new ResponseDTO { Status = "Error", Message = "Invalid client request" });


        var principal = _authService.GetPrincipalFromExpiredToken(expiredToken, _configuration);

        var userId = principal.FindFirst(ClaimTypes.NameIdentifier);
        if (userId?.Value! == null)
            return BadRequest(new ResponseDTO { Status = "Error", Message = "Invalid access token/refresh token" });


        var user = await _context.Users.FirstOrDefaultAsync(user => user.Id == userId.Value);
        if (user == null || user.RefreshToken == expiredToken || user.RefreshTokenExpirytime <= DateTime.Now)
            return BadRequest(new ResponseDTO { Status = "Error", Message = "Invalid access token/refresh token" });


        try
        {
            var newAccessToken = _authService.GererateAccessToken(
                principal.Claims.ToList(), _configuration);

            var newRefreshToken = _authService.GenerateRefreshToken();

            user.RefreshToken = newRefreshToken;

            _context.Users.Update(user);
            await _context.SaveChangesAsync();

            return Ok(new
            {
                Token = new JwtSecurityTokenHandler().WriteToken((SecurityToken)newAccessToken),
                RefreshToken = newRefreshToken
            });
        }
        catch (Exception ex)
        {
            return this.InternalServerError(ex);
        }

    }
}