namespace IlmAI.Api.Services.Default;

using IlmAI.Api.Models.Response.Quran;

public class QuranService : IQuranService
{
    private readonly ILogger<QuranService> _logger;

    public QuranService(ILogger<QuranService> logger)
    {
        _logger = logger;
    }

    public Task<List<AyahResponse>> SearchAsync(string query, string language)
    {
        // TODO: Implement Qur'an search via AI server semantic search
        _logger.LogInformation("Quran search: {Query} ({Language})", query, language);
        return Task.FromResult(new List<AyahResponse>());
    }

    public Task<AyahResponse?> GetAyahAsync(int surah, int ayah, string language)
    {
        // TODO: Implement ayah lookup from database
        _logger.LogInformation("Get ayah: {Surah}:{Ayah} ({Language})", surah, ayah, language);
        return Task.FromResult<AyahResponse?>(null);
    }

    public Task<List<SurahResponse>> GetSurahsAsync()
    {
        // TODO: Implement surah list from database
        return Task.FromResult(new List<SurahResponse>());
    }
}
