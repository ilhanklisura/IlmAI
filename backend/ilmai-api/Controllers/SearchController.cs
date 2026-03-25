namespace IlmAI.Api.Controllers;

using IlmAI.Api.Models.Request.Search;
using Microsoft.AspNetCore.Mvc;

[ApiController]
[Route("api/[controller]")]
public class SearchController : ControllerBase
{
    private readonly IHttpClientFactory _httpClientFactory;

    public SearchController(IHttpClientFactory httpClientFactory) { _httpClientFactory = httpClientFactory; }

    [HttpPost]
    public async Task<IActionResult> Search([FromBody] SearchRequest request)
    {
        try
        {
            var client = _httpClientFactory.CreateClient("AIServer");
            var response = await client.PostAsJsonAsync("/api/search", new { question = request.Query, language = request.Language });
            var result = await response.Content.ReadAsStringAsync();
            return Content(result, "application/json");
        }
        catch (Exception ex)
        {
            return StatusCode(503, new { message = "AI server unavailable", error = ex.Message });
        }
    }
}
