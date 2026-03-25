namespace IlmAI.Api.Models.Response.Chat;

public class ChatSessionResponse
{
    public Guid Id { get; set; }
    public string? Title { get; set; }
    public DateTime CreatedAt { get; set; }
    public DateTime UpdatedAt { get; set; }
    public int MessageCount { get; set; }
}

public class ChatMessageResponse
{
    public Guid Id { get; set; }
    public string Role { get; set; } = "";
    public string Content { get; set; } = "";
    public List<SourceCitation>? Sources { get; set; }
    public DateTime CreatedAt { get; set; }
}

public class SourceCitation
{
    public string Type { get; set; } = ""; // "quran", "hadith", "tafsir"
    public string Reference { get; set; } = ""; // e.g., "2:286" or "Bukhari 1234"
    public string Content { get; set; } = "";
    public double Score { get; set; }
}

public class AskResponse
{
    public Guid SessionId { get; set; }
    public bool HasAnswer { get; set; }
    public string Answer { get; set; } = "";
    public double ConfidenceScore { get; set; }
    public List<SourceCitation> Sources { get; set; } = new();
}
