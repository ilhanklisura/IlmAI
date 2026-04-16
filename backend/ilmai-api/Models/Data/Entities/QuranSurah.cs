namespace IlmAI.Api.Models.Data.Entities;

using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

[Table("quran_surahs")]
public class QuranSurah
{
    [Key]
    [Column("id")]
    public int Id { get; set; }

    [Column("surah_number")]
    public int SurahNumber { get; set; }

    [Column("name_arabic")]
    public string? NameArabic { get; set; }

    [Column("name_transliteration")]
    public string? NameTransliteration { get; set; }

    [Column("name_bosnian")]
    public string? NameBosnian { get; set; }

    [Column("name_english")]
    public string? NameEnglish { get; set; }

    [Column("revelation_type")]
    public string? RevelationType { get; set; }

    [Column("ayah_count")]
    public int AyahCount { get; set; }
}
