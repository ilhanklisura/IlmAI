namespace IlmAI.Api.Controllers;

using IlmAI.Api.Services;
using Microsoft.AspNetCore.Mvc;

[ApiController]
[Route("api/[controller]")]
public class DailyController : ControllerBase
{
    private readonly IDailyService _dailyService;

    public DailyController(IDailyService dailyService) { _dailyService = dailyService; }

    [HttpGet]
    public async Task<IActionResult> GetDailyContent([FromQuery] string language = "bs")
    {
        var content = await _dailyService.GetDailyContentAsync(language);
        return Ok(content);
    }
}
