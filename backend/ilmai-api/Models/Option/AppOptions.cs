namespace IlmAI.Api.Models.Option;

public class AppOptions
{
    public string TokenSecret { get; set; } = "";
    public int TokenExpirationMinutes { get; set; } = 1440;
    public string AIServerHost { get; set; } = "http://localhost:5031";
}
