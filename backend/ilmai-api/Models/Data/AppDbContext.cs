namespace IlmAI.Api.Models.Data;

using IlmAI.Api.Models.Data.Entities;
using Microsoft.EntityFrameworkCore;

public class AppDbContext : DbContext
{
    public AppDbContext(DbContextOptions<AppDbContext> options) : base(options) { }

    public DbSet<User> Users => Set<User>();
    public DbSet<Role> Roles => Set<Role>();
    public DbSet<UserRole> UserRoles => Set<UserRole>();
    public DbSet<UserSettings> UserSettings => Set<UserSettings>();
    public DbSet<ChatSession> ChatSessions => Set<ChatSession>();
    public DbSet<ChatMessage> ChatMessages => Set<ChatMessage>();
    public DbSet<FavoriteItem> FavoriteItems => Set<FavoriteItem>();
    public DbSet<SearchHistory> SearchHistory => Set<SearchHistory>();
    public DbSet<SystemLog> SystemLogs => Set<SystemLog>();

    // Knowledge Base Context
    public DbSet<QuranAyah> QuranAyahs => Set<QuranAyah>();
    public DbSet<QuranSurah> QuranSurahs => Set<QuranSurah>();
    public DbSet<Hadith> Hadiths => Set<Hadith>();
    public DbSet<HadithCollection> HadithCollections => Set<HadithCollection>();

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);

        // User
        modelBuilder.Entity<User>(e =>
        {
            e.ToTable("users");
            e.HasKey(u => u.Id);
            e.HasIndex(u => u.Username).IsUnique();
            e.HasIndex(u => u.Email).IsUnique();
            e.HasQueryFilter(u => !u.IsDeleted);
        });

        // Role
        modelBuilder.Entity<Role>(e =>
        {
            e.ToTable("roles");
            e.HasKey(r => r.Id);
            e.HasIndex(r => r.Name).IsUnique();
        });

        // UserRole
        modelBuilder.Entity<UserRole>(e =>
        {
            e.ToTable("user_roles");
            e.HasKey(ur => new { ur.UserId, ur.RoleId });
            e.HasOne(ur => ur.User).WithMany(u => u.UserRoles).HasForeignKey(ur => ur.UserId);
            e.HasOne(ur => ur.Role).WithMany(r => r.UserRoles).HasForeignKey(ur => ur.RoleId);
        });

        // UserSettings
        modelBuilder.Entity<UserSettings>(e =>
        {
            e.ToTable("user_settings");
            e.HasKey(s => s.Id);
            e.HasOne(s => s.User).WithOne(u => u.Settings).HasForeignKey<UserSettings>(s => s.UserId);
        });

        // ChatSession
        modelBuilder.Entity<ChatSession>(e =>
        {
            e.ToTable("chat_sessions");
            e.HasKey(s => s.Id);
            e.HasOne(s => s.User).WithMany(u => u.ChatSessions).HasForeignKey(s => s.UserId);
        });

        // ChatMessage
        modelBuilder.Entity<ChatMessage>(e =>
        {
            e.ToTable("chat_messages");
            e.HasKey(m => m.Id);
            e.HasOne(m => m.Session).WithMany(s => s.Messages).HasForeignKey(m => m.SessionId);
        });

        // FavoriteItem
        modelBuilder.Entity<FavoriteItem>(e =>
        {
            e.ToTable("favorite_items");
            e.HasKey(f => f.Id);
            e.HasOne(f => f.User).WithMany(u => u.Favorites).HasForeignKey(f => f.UserId);
        });

        // SearchHistory
        modelBuilder.Entity<SearchHistory>(e =>
        {
            e.ToTable("search_history");
            e.HasKey(h => h.Id);
            e.HasOne(h => h.User).WithMany().HasForeignKey(h => h.UserId);
        });

        // SystemLog
        modelBuilder.Entity<SystemLog>(e =>
        {
            e.ToTable("system_logs");
            e.HasKey(l => l.Id);
            e.HasIndex(l => l.Level);
            e.HasIndex(l => l.CreatedAt);
        });

        // QuranAyah
        modelBuilder.Entity<QuranAyah>(e =>
        {
            e.ToTable("quran_ayahs");
            e.HasKey(a => a.Id);
        });

        // QuranSurah
        modelBuilder.Entity<QuranSurah>(e =>
        {
            e.ToTable("quran_surahs");
            e.HasKey(s => s.Id);
            e.HasIndex(s => s.SurahNumber).IsUnique();
        });

        // HadithCollection
        modelBuilder.Entity<HadithCollection>(e =>
        {
            e.ToTable("hadith_collections");
            e.HasKey(h => h.Id);
            e.HasIndex(h => h.Name).IsUnique();
        });

        // Hadith
        modelBuilder.Entity<Hadith>(e =>
        {
            e.ToTable("hadiths");
            e.HasKey(h => h.Id);
            e.HasOne(h => h.Collection)
             .WithMany()
             .HasForeignKey(h => h.CollectionId);
        });
    }
}
