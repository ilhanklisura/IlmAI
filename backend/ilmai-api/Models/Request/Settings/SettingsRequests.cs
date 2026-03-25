namespace IlmAI.Api.Models.Request.Settings;

public class UpdateSettingsRequest
{
    public string? Theme { get; set; }
    public string? Language { get; set; }
    public bool? NotificationsEnabled { get; set; }
}
