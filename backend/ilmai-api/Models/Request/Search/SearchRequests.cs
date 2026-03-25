namespace IlmAI.Api.Models.Request.Search;

public class SearchRequest
{
    public string Query { get; set; } = "";
    public string Language { get; set; } = "bs";
    public string? Category { get; set; } // "quran", "hadith", "tafsir", or null for all
}
