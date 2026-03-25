namespace IlmAI.Api.Services.Default;

using IlmAI.Api.Models.Data;
using IlmAI.Api.Models.Data.Entities;
using IlmAI.Api.Models.Request.Settings;
using Microsoft.EntityFrameworkCore;

public class SettingsService : ISettingsService
{
    private readonly AppDbContext _context;

    public SettingsService(AppDbContext context) { _context = context; }

    public async Task<object?> GetSettingsAsync(Guid userId)
    {
        var settings = await _context.UserSettings.FirstOrDefaultAsync(s => s.UserId == userId);
        if (settings == null) return new { theme = "dark", language = "bs", notificationsEnabled = true };

        return new
        {
            theme = settings.Theme,
            language = settings.Language,
            notificationsEnabled = settings.NotificationsEnabled
        };
    }

    public async Task UpdateSettingsAsync(Guid userId, UpdateSettingsRequest request)
    {
        var settings = await _context.UserSettings.FirstOrDefaultAsync(s => s.UserId == userId);
        if (settings == null)
        {
            settings = new UserSettings { UserId = userId };
            _context.UserSettings.Add(settings);
        }

        if (request.Theme != null) settings.Theme = request.Theme;
        if (request.Language != null) settings.Language = request.Language;
        if (request.NotificationsEnabled.HasValue) settings.NotificationsEnabled = request.NotificationsEnabled.Value;
        settings.UpdatedAt = DateTime.UtcNow;

        await _context.SaveChangesAsync();
    }
}
