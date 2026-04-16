namespace IlmAI.Api.Models.Data.Entities;

using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

[Table("hadiths")]
public class Hadith
{
    [Key]
    [Column("id")]
    public Guid Id { get; set; }

    [Column("collection_id")]
    public int CollectionId { get; set; }

    [Column("hadith_number")]
    public int HadithNumber { get; set; }

    [Column("chapter")]
    public string? Chapter { get; set; }

    [Column("text_arabic")]
    public string? TextArabic { get; set; }

    [Column("text_bosnian")]
    public string? TextBosnian { get; set; }

    [Column("text_english")]
    public string? TextEnglish { get; set; }

    [Column("grade")]
    public string? Grade { get; set; }

    [Column("narrator")]
    public string? Narrator { get; set; }

    public HadithCollection? Collection { get; set; }
}
