namespace IlmAI.Api.Models.Response.Admin;

public class AdminStatsResponse
{
    public int TotalUsers { get; set; }
    public int ActiveUsers24h { get; set; }
    public int TotalMessages { get; set; }
    public int Messages24h { get; set; }
    public int TotalSessions { get; set; }
    public List<TopTopicDto> TopTopics { get; set; } = new();
}

public class TopTopicDto
{
    public string Title { get; set; } = "";
    public int Count { get; set; }
}

public class AdminUserResponse
{
    public Guid Id { get; set; }
    public string Username { get; set; } = "";
    public string Email { get; set; } = "";
    public string? FullName { get; set; }
    public bool IsEmailVerified { get; set; }
    public bool IsActive { get; set; }
    public DateTime CreatedAt { get; set; }
    public List<string> Roles { get; set; } = new();
    public DateTime? LastActiveAt { get; set; }
}

public class AdminAnalyticsResponse
{
    public List<DailyActivityDto> DailyActivity { get; set; } = new();
    public List<TopQuestionDto> TopQuestions { get; set; } = new();
}

public class DailyActivityDto
{
    public DateTime Date { get; set; }
    public int MessageCount { get; set; }
}

public class TopQuestionDto
{
    public string Text { get; set; } = "";
    public int Count { get; set; }
}
