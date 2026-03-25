namespace IlmAI.Api.Models.Response.Tafsir;

public class TafsirResponse
{
    public Guid Id { get; set; }
    public string SourceName { get; set; } = "";
    public int SurahNumber { get; set; }
    public int AyahStart { get; set; }
    public int AyahEnd { get; set; }
    public string Content { get; set; } = "";
}
