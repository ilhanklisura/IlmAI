namespace IlmAI.Api.Models.Data.Entities;

public class User
{
    public Guid Id { get; set; } = Guid.NewGuid();
    public string Username { get; set; } = "";
    public string Email { get; set; } = "";
    public string PasswordHash { get; set; } = "";
    public string? FirstName { get; set; }
    public string? LastName { get; set; }
    public string PreferredLanguage { get; set; } = "bs";
    public bool IsActive { get; set; } = true;
    public bool IsDeleted { get; set; } = false;
    public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
    public DateTime UpdatedAt { get; set; } = DateTime.UtcNow;

    // Email Verification
    public bool IsEmailVerified { get; set; } = false;
    public string? EmailVerificationCode { get; set; }

    public ICollection<UserRole> UserRoles { get; set; } = new List<UserRole>();
    public UserSettings? Settings { get; set; }
    public ICollection<ChatSession> ChatSessions { get; set; } = new List<ChatSession>();
    public ICollection<FavoriteItem> Favorites { get; set; } = new List<FavoriteItem>();
}
