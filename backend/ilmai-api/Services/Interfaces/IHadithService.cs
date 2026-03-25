namespace IlmAI.Api.Services;

using IlmAI.Api.Models.Response.Hadith;

public interface IHadithService : IService
{
    Task<List<HadithResponse>> SearchAsync(string query, string language);
    Task<HadithResponse?> GetByIdAsync(Guid id);
}

