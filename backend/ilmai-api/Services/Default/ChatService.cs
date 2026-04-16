namespace IlmAI.Api.Services.Default;

using System.Net.Http.Json;
using System.Text.Json;
using IlmAI.Api.Models.Data;
using IlmAI.Api.Models.Data.Entities;
using IlmAI.Api.Models.Request.Chat;
using IlmAI.Api.Models.Response.Chat;
using Microsoft.EntityFrameworkCore;

public class ChatService : IChatService
{
    private readonly AppDbContext _context;
    private readonly IHttpClientFactory _httpClientFactory;
    private readonly ILogger<ChatService> _logger;

    public ChatService(AppDbContext context, IHttpClientFactory httpClientFactory, ILogger<ChatService> logger)
    {
        _context = context;
        _httpClientFactory = httpClientFactory;
        _logger = logger;
    }

    public async Task<AskResponse> AskAsync(Guid userId, SendMessageRequest request)
    {
        // Get or create session
        ChatSession session;
        if (request.SessionId.HasValue)
        {
            session = await _context.ChatSessions
                .FirstOrDefaultAsync(s => s.Id == request.SessionId.Value && s.UserId == userId)
                ?? throw new InvalidOperationException("Session not found");
        }
        else
        {
            var title = await GetAiGeneratedTitleAsync(request.Question, request.Language);
            session = new ChatSession
            {
                UserId = userId,
                Title = title
            };
            _context.ChatSessions.Add(session);
            await _context.SaveChangesAsync();
        }

        // Extract past chat history (last 5 messages)
        var historyOptions = new List<object>();
        if (request.SessionId.HasValue)
        {
            var priorMessages = await _context.ChatMessages
                .Where(m => m.SessionId == session.Id)
                .OrderByDescending(m => m.CreatedAt)
                .Take(5)
                .ToListAsync();

            historyOptions = priorMessages
                .OrderBy(m => m.CreatedAt)
                .Select(m => new { role = m.Role, content = m.Content })
                .Cast<object>()
                .ToList();
        }

        // Save user message (Done after history extraction to avoid sending duplicate question)
        var userMessage = new ChatMessage
        {
            SessionId = session.Id,
            Role = "user",
            Content = request.Question
        };
        _context.ChatMessages.Add(userMessage);
        await _context.SaveChangesAsync();

        // Call AI Server
        try
        {
            var client = _httpClientFactory.CreateClient("AIServer");
            var aiRequest = new { question = request.Question, language = request.Language, history = historyOptions };
            var response = await client.PostAsJsonAsync("/api/rag/global", aiRequest);

            if (response.IsSuccessStatusCode)
            {
                var aiResponse = await response.Content.ReadFromJsonAsync<AiServerResponse>();
                if (aiResponse != null)
                {
                    var sources = aiResponse.Sources?.Select(s => new SourceCitation
                    {
                        Type = s.DocumentTitle?.Contains("Quran") == true ? "quran"
                             : s.DocumentTitle?.Contains("Hadith") == true ? "hadith"
                             : "tafsir",
                        Reference = s.DocumentTitle ?? "",
                        Content = s.Content ?? "",
                        Score = s.Score
                    }).ToList() ?? new();

                    // Save assistant message
                    var assistantMessage = new ChatMessage
                    {
                        SessionId = session.Id,
                        Role = "assistant",
                        Content = aiResponse.Answer ?? "",
                        SourcesJson = JsonSerializer.Serialize(sources)
                    };
                    _context.ChatMessages.Add(assistantMessage);
                    session.UpdatedAt = DateTime.UtcNow;
                    await _context.SaveChangesAsync();

                    return new AskResponse
                    {
                        SessionId = session.Id,
                        HasAnswer = aiResponse.HasAnswer,
                        Answer = aiResponse.Answer ?? "",
                        ConfidenceScore = aiResponse.ConfidenceScore,
                        Sources = sources
                    };
                }
            }

            _logger.LogWarning("AI Server returned non-success: {StatusCode}", response.StatusCode);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Failed to call AI Server");
        }

        // Fallback response
        return new AskResponse
        {
            SessionId = session.Id,
            HasAnswer = false,
            Answer = "AI server is currently unavailable. Please try again later.",
            ConfidenceScore = 0,
            Sources = new()
        };
    }

    public async Task<List<ChatSessionResponse>> GetSessionsAsync(Guid userId)
    {
        return await _context.ChatSessions
            .Where(s => s.UserId == userId)
            .OrderByDescending(s => s.UpdatedAt)
            .Select(s => new ChatSessionResponse
            {
                Id = s.Id,
                Title = s.Title,
                CreatedAt = s.CreatedAt,
                UpdatedAt = s.UpdatedAt,
                MessageCount = s.Messages.Count
            })
            .ToListAsync();
    }

    public async Task<List<ChatMessageResponse>> GetMessagesAsync(Guid userId, Guid sessionId)
    {
        var session = await _context.ChatSessions
            .FirstOrDefaultAsync(s => s.Id == sessionId && s.UserId == userId);

        if (session == null) return new();

        var messages = await _context.ChatMessages
            .Where(m => m.SessionId == sessionId)
            .OrderBy(m => m.CreatedAt)
            .Select(m => new { m.Id, m.Role, m.Content, m.SourcesJson, m.CreatedAt })
            .ToListAsync();

        return messages.Select(m => new ChatMessageResponse
        {
            Id = m.Id,
            Role = m.Role,
            Content = m.Content,
            Sources = m.SourcesJson != null
                ? JsonSerializer.Deserialize<List<SourceCitation>>(m.SourcesJson)
                : null,
            CreatedAt = m.CreatedAt
        }).ToList();
    }

    public async Task DeleteSessionAsync(Guid userId, Guid sessionId)
    {
        var session = await _context.ChatSessions
            .Include(s => s.Messages)
            .FirstOrDefaultAsync(s => s.Id == sessionId && s.UserId == userId);

        if (session == null)
            throw new InvalidOperationException("Session not found");

        _context.ChatMessages.RemoveRange(session.Messages);
        _context.ChatSessions.Remove(session);

        await _context.SaveChangesAsync();
    }

    public async Task UpdateTitleAsync(Guid sessionId, string title)
    {
        if (string.IsNullOrWhiteSpace(title))
            throw new ArgumentException("Title cannot be empty");

        if (title.Length > 100)
            title = title.Substring(0, 100);

        var session = await _context.ChatSessions.FindAsync(sessionId);

        if (session == null)
            throw new Exception("Session not found");

        session.Title = title;

        await _context.SaveChangesAsync();
    }

    private async Task<string> GetAiGeneratedTitleAsync(string question, string language)
    {
        try
        {
            var client = _httpClientFactory.CreateClient("AIServer");
            var titleRequest = new { question, language };
            
            // Short timeout for title generation to avoid blocking the main chat
            var response = await client.PostAsJsonAsync("/api/title", titleRequest);
            
            if (response.IsSuccessStatusCode)
            {
                var result = await response.Content.ReadFromJsonAsync<JsonElement>();
                if (result.TryGetProperty("title", out var titleProp))
                {
                    return titleProp.GetString() ?? question[..Math.Min(question.Length, 50)];
                }
            }
        }
        catch (Exception ex)
        {
            _logger.LogWarning(ex, "Failed to get AI generated title, falling back to truncated question");
        }

        // Fallback: Truncate question
        return question.Length > 50 ? question[..50] + "..." : question;
    }

    // Internal model for AI Server response
    private class AiServerResponse
    {
        public bool HasAnswer { get; set; }
        public string? Answer { get; set; }
        public double ConfidenceScore { get; set; }
        public bool LowConfidence { get; set; }
        public List<AiServerSource>? Sources { get; set; }
    }

    private class AiServerSource
    {
        public string? DocumentTitle { get; set; }
        public string? Content { get; set; }
        public string? StartTime { get; set; }
        public string? EndTime { get; set; }
        public double Score { get; set; }
    }
}
