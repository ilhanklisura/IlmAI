namespace IlmAI.Api.Controllers;

using IlmAI.Api.Models.Request.Chat;
using IlmAI.Api.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;

[ApiController]
[Route("api/[controller]")]
[Authorize]
public class ChatController : ControllerBase
{
    private readonly IChatService _chatService;

    public ChatController(IChatService chatService) { _chatService = chatService; }

    private Guid GetUserId() => Guid.Parse(User.FindFirstValue(ClaimTypes.NameIdentifier)!);

    [HttpPost("send")]
    public async Task<IActionResult> SendMessage([FromBody] SendMessageRequest request)
    {
        var result = await _chatService.AskAsync(GetUserId(), request);
        return Ok(result);
    }

    [HttpGet("sessions")]
    public async Task<IActionResult> GetSessions()
    {
        var sessions = await _chatService.GetSessionsAsync(GetUserId());
        return Ok(sessions);
    }

    [HttpGet("sessions/{sessionId}/messages")]
    public async Task<IActionResult> GetMessages(Guid sessionId)
    {
        var messages = await _chatService.GetMessagesAsync(GetUserId(), sessionId);
        return Ok(messages);
    }
}
