namespace IlmAI.Api.Models.Response.Daily;

public class DailyContentResponse
{
    public DailyItem Ayah { get; set; } = new();
    public DailyItem Hadith { get; set; } = new();
    public DateTime Date { get; set; }
}

public class DailyItem
{
    public string? SurahName { get; set; }
    public string? AyahReference { get; set; }
    public string? HadithCollection { get; set; }
    public int? HadithNumber { get; set; }
    public string TextArabic { get; set; } = "";
    public string TextTranslation { get; set; } = "";
    public string? Explanation { get; set; }
}
