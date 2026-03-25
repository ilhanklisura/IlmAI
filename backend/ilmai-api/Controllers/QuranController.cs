namespace IlmAI.Api.Controllers;

using IlmAI.Api.Services;
using Microsoft.AspNetCore.Mvc;

[ApiController]
[Route("api/[controller]")]
public class QuranController : ControllerBase
{
    private readonly IQuranService _quranService;

    public QuranController(IQuranService quranService) { _quranService = quranService; }

    [HttpGet("search")]
    public async Task<IActionResult> Search([FromQuery] string query, [FromQuery] string language = "bs")
    {
        var results = await _quranService.SearchAsync(query, language);
        return Ok(results);
    }

    [HttpGet("{surah}/{ayah}")]
    public async Task<IActionResult> GetAyah(int surah, int ayah, [FromQuery] string language = "bs")
    {
        var result = await _quranService.GetAyahAsync(surah, ayah, language);
        return result != null ? Ok(result) : NotFound();
    }

    [HttpGet("surahs")]
    public async Task<IActionResult> GetSurahs()
    {
        var surahs = await _quranService.GetSurahsAsync();
        return Ok(surahs);
    }
}
