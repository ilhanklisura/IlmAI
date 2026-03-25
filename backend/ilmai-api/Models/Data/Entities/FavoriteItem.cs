namespace IlmAI.Api.Models.Data.Entities;

public class FavoriteItem
{
    public Guid Id { get; set; } = Guid.NewGuid();
    public Guid UserId { get; set; }
    public User User { get; set; } = null!;
    public string ItemType { get; set; } = ""; // "ayah", "hadith", "tafsir"
    public string ItemId { get; set; } = "";
    public string? Note { get; set; }
    public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
}
