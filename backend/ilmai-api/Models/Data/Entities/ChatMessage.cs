namespace IlmAI.Api.Models.Data.Entities;

public class ChatMessage
{
    public Guid Id { get; set; } = Guid.NewGuid();
    public Guid SessionId { get; set; }
    public ChatSession Session { get; set; } = null!;
    public string Role { get; set; } = "user"; // "user" or "assistant"
    public string Content { get; set; } = "";
    public string? SourcesJson { get; set; } // JSON string of source citations
    public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
}
