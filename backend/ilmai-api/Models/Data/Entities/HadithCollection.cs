namespace IlmAI.Api.Models.Data.Entities;

using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

[Table("hadith_collections")]
public class HadithCollection
{
    [Key]
    [Column("id")]
    public int Id { get; set; }

    [Column("name")]
    public string Name { get; set; } = null!;

    [Column("name_arabic")]
    public string? NameArabic { get; set; }

    [Column("author")]
    public string? Author { get; set; }

    [Column("hadith_count")]
    public int? HadithCount { get; set; }
}
