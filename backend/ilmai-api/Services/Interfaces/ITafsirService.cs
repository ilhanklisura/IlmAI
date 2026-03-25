namespace IlmAI.Api.Services;

using IlmAI.Api.Models.Response.Tafsir;

public interface ITafsirService : IService
{
    Task<List<TafsirResponse>> GetForAyahAsync(int surah, int ayahStart, int ayahEnd);
}
