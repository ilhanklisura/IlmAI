namespace IlmAI.Api.Services.Default;

using IlmAI.Api.Models.Response.Hadith;

public class HadithService : IHadithService
{
    private readonly ILogger<HadithService> _logger;

    public HadithService(ILogger<HadithService> logger) { _logger = logger; }

    public Task<List<HadithResponse>> SearchAsync(string query, string language)
    {
        _logger.LogInformation("Hadith search: {Query} ({Language})", query, language);
        return Task.FromResult(new List<HadithResponse>());
    }

    public Task<HadithResponse?> GetByIdAsync(Guid id)
    {
        return Task.FromResult<HadithResponse?>(null);
    }
}
