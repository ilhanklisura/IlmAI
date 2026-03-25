namespace IlmAI.AI.Data.Entities;
using Pgvector;
public class Document
{
    public Guid Id { get; set; } = Guid.NewGuid();
    public string Title { get; set; } = "";
    public string SourceType { get; set; } = ""; // "quran", "hadith", "tafsir", "other"
    public string Language { get; set; } = "bs";
    public string Status { get; set; } = "pending"; // "pending", "indexed", "error"
    public string? FileName { get; set; }
    public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
    public DateTime UpdatedAt { get; set; } = DateTime.UtcNow;
    public ICollection<DocumentChunk> Chunks { get; set; } = new List<DocumentChunk>();
}
public class DocumentChunk
{
    public Guid Id { get; set; } = Guid.NewGuid();
    public Guid DocumentId { get; set; }
    public Document Document { get; set; } = null!;
    public int ChunkIndex { get; set; }
    public string Content { get; set; } = "";
    public Vector? Embedding { get; set; }
    public string? MetadataJson { get; set; }
    public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
}
public class QuranChunk
{
    public Guid Id { get; set; } = Guid.NewGuid();
    public int SurahNumber { get; set; }
    public int AyahStart { get; set; }
    public int AyahEnd { get; set; }
    public string Content { get; set; } = "";
    public string Language { get; set; } = "bs";
    public Vector? Embedding { get; set; }
    public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
}
public class HadithChunk
{
    public Guid Id { get; set; } = Guid.NewGuid();
    public int CollectionId { get; set; }
    public int HadithNumber { get; set; }
    public string Content { get; set; } = "";
    public string Language { get; set; } = "bs";
    public Vector? Embedding { get; set; }
    public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
}
public class TafsirChunk
{
    public Guid Id { get; set; } = Guid.NewGuid();
    public int SourceId { get; set; }
    public int SurahNumber { get; set; }
    public int AyahStart { get; set; }
    public int AyahEnd { get; set; }
    public string Content { get; set; } = "";
    public string Language { get; set; } = "bs";
    public Vector? Embedding { get; set; }
    public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
}
