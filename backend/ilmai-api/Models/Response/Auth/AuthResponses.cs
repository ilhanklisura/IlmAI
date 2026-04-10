namespace IlmAI.Api.Models.Response.Auth;

public class TokenResponse
{
    public string Token { get; set; } = "";
    public DateTime ExpiresAt { get; set; }
    public UserResponse User { get; set; } = null!;
}

public class UserResponse
{
    public Guid Id { get; set; }
    public string Username { get; set; } = "";
    public string Email { get; set; } = "";
    public string? FirstName { get; set; }
    public string? LastName { get; set; }
    public string PreferredLanguage { get; set; } = "bs";
    public bool IsEmailVerified { get; set; }
    public List<string> Roles { get; set; } = new();
}
