namespace IlmAI.Api.Services.Default;

using IlmAI.Api.Models.Data;
using IlmAI.Api.Models.Data.Entities;
using Microsoft.EntityFrameworkCore;

public class SeedService
{
    public static async Task SeedUsersAsync(IServiceProvider services)
    {
        using var scope = services.CreateScope();
        var context = scope.ServiceProvider.GetRequiredService<AppDbContext>();
        var logger = scope.ServiceProvider.GetRequiredService<ILogger<SeedService>>();

        // Ensure database table for logs exists (raw SQL to avoid migration warnings)
        logger.LogInformation("Seed: Ensuring logs table exists...");
        await context.Database.ExecuteSqlRawAsync(@"
            CREATE TABLE IF NOT EXISTS system_logs (
                id UUID PRIMARY KEY,
                level VARCHAR(20) NOT NULL,
                message TEXT NOT NULL,
                source TEXT,
                exception TEXT,
                user_id TEXT,
                created_at TIMESTAMP WITHOUT TIME ZONE NOT NULL
            );
            CREATE INDEX IF NOT EXISTS idx_system_logs_level ON system_logs (level);
            CREATE INDEX IF NOT EXISTS idx_system_logs_created_at ON system_logs (created_at);
        ");

        // Seed initial logs if empty
        if (!await context.SystemLogs.AnyAsync())
        {
            context.SystemLogs.AddRange(new List<SystemLog>
            {
                new SystemLog { Level = "Info", Message = "Sistem uspješno pokrenut.", Source = "System" },
                new SystemLog { Level = "Info", Message = "Admin panel konfigurisan.", Source = "AdminUI" },
                new SystemLog { Level = "Warning", Message = "Provjera verifikacije korisnika u toku...", Source = "AuthService" }
            });
            await context.SaveChangesAsync();
        }

        // Check if admin already exists
        var existingAdmin = await context.Users.FirstOrDefaultAsync(u => u.Username == "admin");
        if (existingAdmin != null)
        {
            if (!existingAdmin.IsEmailVerified)
            {
                existingAdmin.IsEmailVerified = true;
                await context.SaveChangesAsync();
                logger.LogInformation("Seed: Existing admin was not verified, marking as verified now.");
            }
            else
            {
                logger.LogInformation("Seed: Admin already exists and is verified.");
            }
            return;
        }

        logger.LogInformation("Seed: Creating admin and test users...");

        var adminRole = await context.Roles.FirstOrDefaultAsync(r => r.Name == "admin");
        var userRole = await context.Roles.FirstOrDefaultAsync(r => r.Name == "user");

        if (adminRole == null || userRole == null)
        {
            logger.LogWarning("Seed: Roles not found. Run migrations first.");
            return;
        }

        // Admin user
        var admin = new User
        {
            Username = "admin",
            Email = "admin@ilmai.ba",
            PasswordHash = BCrypt.Net.BCrypt.HashPassword("Admin123!"),
            FirstName = "Admin",
            LastName = "IlmAI",
            PreferredLanguage = "bs",
            IsEmailVerified = true
        };
        admin.UserRoles.Add(new UserRole { Role = adminRole });
        admin.UserRoles.Add(new UserRole { Role = userRole });
        admin.Settings = new UserSettings { Language = "bs", Theme = "dark" };
        context.Users.Add(admin);

        // Test user
        var testUser = new User
        {
            Username = "korisnik",
            Email = "korisnik@ilmai.ba",
            PasswordHash = BCrypt.Net.BCrypt.HashPassword("User123!"),
            FirstName = "Test",
            LastName = "Korisnik",
            PreferredLanguage = "bs",
            IsEmailVerified = true
        };
        testUser.UserRoles.Add(new UserRole { Role = userRole });
        testUser.Settings = new UserSettings { Language = "bs", Theme = "dark" };
        context.Users.Add(testUser);

        await context.SaveChangesAsync();
        logger.LogInformation("Seed: Created admin (admin/Admin123!) and test user (korisnik/User123!)");
    }
}
