namespace IlmAI.Api.Models.Data.Entities;

using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

[Table("quran_ayahs")]
public class QuranAyah
{
    [Key]
    [Column("id")]
    public Guid Id { get; set; }

    [Column("surah_number")]
    public int SurahNumber { get; set; }

    [Column("ayah_number")]
    public int AyahNumber { get; set; }

    [Column("text_arabic")]
    public string? TextArabic { get; set; }

    [Column("text_bosnian")]
    public string? TextBosnian { get; set; }

    [Column("text_english")]
    public string? TextEnglish { get; set; }

    [Column("juz")]
    public int? Juz { get; set; }

    [Column("hizb")]
    public int? Hizb { get; set; }

    [Column("page")]
    public int? Page { get; set; }
}
