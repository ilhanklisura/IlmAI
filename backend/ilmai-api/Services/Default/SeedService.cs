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

        // Check if admin already exists
        if (await context.Users.AnyAsync(u => u.Username == "admin"))
        {
            logger.LogInformation("Seed: Users already seeded, skipping.");
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
            PreferredLanguage = "bs"
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
            PreferredLanguage = "bs"
        };
        testUser.UserRoles.Add(new UserRole { Role = userRole });
        testUser.Settings = new UserSettings { Language = "bs", Theme = "dark" };
        context.Users.Add(testUser);

        await context.SaveChangesAsync();
        logger.LogInformation("Seed: Created admin (admin/Admin123!) and test user (korisnik/User123!)");
    }
}
