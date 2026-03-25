namespace IlmAI.Api.Services;

using IlmAI.Api.Models.Request.Settings;

public interface ISettingsService : IService
{
    Task<object?> GetSettingsAsync(Guid userId);
    Task UpdateSettingsAsync(Guid userId, UpdateSettingsRequest request);
}
