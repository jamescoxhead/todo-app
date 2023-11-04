namespace TodoApp.Application.Configuration.Models;

public class JwtOptions
{
    public const string Key = "Jwt";

    public string Audience { get; set; } = string.Empty;
    public string Issuer { get; set; } = string.Empty;
    public int ExpiresHours { get; set; }
    public string Secret { get; set; } = string.Empty;
}
