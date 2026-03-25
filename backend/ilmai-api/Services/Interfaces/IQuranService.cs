namespace IlmAI.Api.Services;

using IlmAI.Api.Models.Response.Quran;

public interface IQuranService : IService
{
    Task<List<AyahResponse>> SearchAsync(string query, string language);
    Task<AyahResponse?> GetAyahAsync(int surah, int ayah, string language);
    Task<List<SurahResponse>> GetSurahsAsync();
}
