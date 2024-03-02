using System.ComponentModel.DataAnnotations;

namespace dto;

public class RegisterDTO
{
    [EmailAddress]
    public required string Email { get; set; }
    public required string Password { get; set; }
}