namespace IlmAI.Api.Models.Data.Entities;

public class SearchHistory
{
    public Guid Id { get; set; } = Guid.NewGuid();
    public Guid UserId { get; set; }
    public User User { get; set; } = null!;
    public string Query { get; set; } = "";
    public int ResultsCount { get; set; }
    public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
}
