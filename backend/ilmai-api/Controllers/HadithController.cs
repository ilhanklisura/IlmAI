namespace IlmAI.Api.Controllers;

using IlmAI.Api.Services;
using Microsoft.AspNetCore.Mvc;

[ApiController]
[Route("api/[controller]")]
public class HadithController : ControllerBase
{
    private readonly IHadithService _hadithService;

    public HadithController(IHadithService hadithService) { _hadithService = hadithService; }

    [HttpGet("search")]
    public async Task<IActionResult> Search([FromQuery] string query, [FromQuery] string language = "bs")
    {
        var results = await _hadithService.SearchAsync(query, language);
        return Ok(results);
    }

    [HttpGet("{id}")]
    public async Task<IActionResult> GetById(Guid id)
    {
        var result = await _hadithService.GetByIdAsync(id);
        return result != null ? Ok(result) : NotFound();
    }
}
