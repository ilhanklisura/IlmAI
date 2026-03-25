namespace IlmAI.AI.Data;
using IlmAI.AI.Data.Entities;
using Microsoft.EntityFrameworkCore;
public class AIDataContext : DbContext
{
    public AIDataContext(DbContextOptions<AIDataContext> options) : base(options) { }
    public DbSet<Document> Documents => Set<Document>();
    public DbSet<DocumentChunk> DocumentChunks => Set<DocumentChunk>();
    public DbSet<QuranChunk> QuranChunks => Set<QuranChunk>();
    public DbSet<HadithChunk> HadithChunks => Set<HadithChunk>();
    public DbSet<TafsirChunk> TafsirChunks => Set<TafsirChunk>();
    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.HasPostgresExtension("vector");
        modelBuilder.Entity<Document>(e => { e.ToTable("documents"); e.HasKey(d => d.Id); });
        modelBuilder.Entity<DocumentChunk>(e =>
        {
            e.ToTable("document_chunks"); e.HasKey(c => c.Id);
            e.HasOne(c => c.Document).WithMany(d => d.Chunks).HasForeignKey(c => c.DocumentId);
            e.HasIndex(c => c.Embedding).HasMethod("hnsw").HasOperators("vector_cosine_ops");
        });
        modelBuilder.Entity<QuranChunk>(e =>
        {
            e.ToTable("quran_chunks"); e.HasKey(c => c.Id);
            e.HasIndex(c => c.Embedding).HasMethod("hnsw").HasOperators("vector_cosine_ops");
        });
        modelBuilder.Entity<HadithChunk>(e =>
        {
            e.ToTable("hadith_chunks"); e.HasKey(c => c.Id);
            e.HasIndex(c => c.Embedding).HasMethod("hnsw").HasOperators("vector_cosine_ops");
        });
        modelBuilder.Entity<TafsirChunk>(e =>
        {
            e.ToTable("tafsir_chunks"); e.HasKey(c => c.Id);
            e.HasIndex(c => c.Embedding).HasMethod("hnsw").HasOperators("vector_cosine_ops");
        });
    }
}
