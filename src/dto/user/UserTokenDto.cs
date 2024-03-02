namespace model.UserTokenDto;

public class UserTokenDto
{
    public string? Token { get; set; }
    public string? Authenticated { get; set; }
    public DateTime Expiration { get; set; }
}