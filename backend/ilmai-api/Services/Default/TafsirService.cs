namespace IlmAI.Api.Services.Default;

using IlmAI.Api.Models.Response.Tafsir;

public class TafsirService : ITafsirService
{
    public Task<List<TafsirResponse>> GetForAyahAsync(int surah, int ayahStart, int ayahEnd)
    {
        return Task.FromResult(new List<TafsirResponse>());
    }
}
