namespace IlmAI.Api.Models.Response.Hadith;

public class HadithResponse
{
    public Guid Id { get; set; }
    public string Collection { get; set; } = "";
    public int HadithNumber { get; set; }
    public string? Chapter { get; set; }
    public string? TextBosnian { get; set; }
    public string? TextEnglish { get; set; }
    public string? Grade { get; set; }
    public string? Narrator { get; set; }
}
