namespace TodoApp.Core.Settings;

public class JwtSettings
{
    public string SecretKey { get; set; }
    public int ExpirationInDays { get; set; }
    public bool ValidateIssuer { get; set; }
    public bool ValidateAudience { get; set; }
} 