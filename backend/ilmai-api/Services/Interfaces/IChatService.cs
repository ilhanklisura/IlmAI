namespace IlmAI.Api.Services;

using IlmAI.Api.Models.Request.Chat;
using IlmAI.Api.Models.Response.Chat;

public interface IChatService : IService
{
    Task<AskResponse> AskAsync(Guid userId, SendMessageRequest request);
    Task<List<ChatSessionResponse>> GetSessionsAsync(Guid userId);
    Task<List<ChatMessageResponse>> GetMessagesAsync(Guid userId, Guid sessionId);
}
