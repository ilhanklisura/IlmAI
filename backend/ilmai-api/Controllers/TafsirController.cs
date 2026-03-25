namespace IlmAI.Api.Controllers;

using IlmAI.Api.Services;
using Microsoft.AspNetCore.Mvc;

[ApiController]
[Route("api/[controller]")]
public class TafsirController : ControllerBase
{
    private readonly ITafsirService _tafsirService;

    public TafsirController(ITafsirService tafsirService) { _tafsirService = tafsirService; }

    [HttpGet("{surah}/{ayahStart}/{ayahEnd}")]
    public async Task<IActionResult> GetTafsir(int surah, int ayahStart, int ayahEnd)
    {
        var results = await _tafsirService.GetForAyahAsync(surah, ayahStart, ayahEnd);
        return Ok(results);
    }

    [HttpGet("{surah}/{ayah}")]
    public async Task<IActionResult> GetTafsirForAyah(int surah, int ayah)
    {
        var results = await _tafsirService.GetForAyahAsync(surah, ayah, ayah);
        return Ok(results);
    }
}
