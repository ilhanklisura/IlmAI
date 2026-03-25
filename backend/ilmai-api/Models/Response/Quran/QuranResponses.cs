namespace IlmAI.Api.Models.Response.Quran;

public class AyahResponse
{
    public int SurahNumber { get; set; }
    public int AyahNumber { get; set; }
    public string? TextArabic { get; set; }
    public string? TextBosnian { get; set; }
    public string? TextEnglish { get; set; }
    public string? SurahName { get; set; }
}

public class SurahResponse
{
    public int SurahNumber { get; set; }
    public string? NameArabic { get; set; }
    public string? NameTransliteration { get; set; }
    public string? NameBosnian { get; set; }
    public string? NameEnglish { get; set; }
    public string? RevelationType { get; set; }
    public int AyahCount { get; set; }
}
