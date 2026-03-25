namespace IlmAI.Api.Models.Request.Chat;

public class SendMessageRequest
{
    public Guid? SessionId { get; set; }
    public string Question { get; set; } = "";
    public string Language { get; set; } = "bs";
}
