namespace IlmAI.Api.Services;

using IlmAI.Api.Models.Response.Daily;

public interface IDailyService : IService
{
    Task<DailyContentResponse> GetDailyContentAsync(string language);
}
