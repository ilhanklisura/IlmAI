namespace IlmAI.AI.Controllers;
using IlmAI.AI.Models.Request;
using IlmAI.AI.Services;
using Microsoft.AspNetCore.Mvc;

[ApiController]
[Route("api/[controller]")]
public class RagController : ControllerBase
{
    private readonly IRagService _ragService;
    public RagController(IRagService ragService) { _ragService = ragService; }

    [HttpPost("global")]
    public async Task<IActionResult> AskGlobal([FromBody] RagRequest request, CancellationToken ct)
    {
        var result = await _ragService.AskGlobalAsync(request.Question, request.Language, ct);
        return Ok(result);
    }
}

[ApiController]
[Route("api/[controller]")]
public class SearchController : ControllerBase
{
    private readonly ISearchService _searchService;
    public SearchController(ISearchService searchService) { _searchService = searchService; }

    [HttpPost]
    public async Task<IActionResult> Search([FromBody] SearchRequest request, CancellationToken ct)
    {
        var results = await _searchService.SearchAsync(request.Question, request.Language, request.TopK > 0 ? request.TopK : 10, ct);
        return Ok(results);
    }
}

[ApiController]
[Route("api/[controller]")]
public class TitleController : ControllerBase
{
    private readonly IChatCompletionService _chat;
    private readonly IPromptBuilderService _promptBuilder;
    public TitleController(IChatCompletionService chat, IPromptBuilderService promptBuilder) 
    { 
        _chat = chat; _promptBuilder = promptBuilder; 
    }

    [HttpPost]
    public async Task<IActionResult> GenerateTitle([FromBody] TitleRequest request, CancellationToken ct)
    {
        var prompt = _promptBuilder.BuildTitlePrompt(request.Question, request.Language);
        var title = await _chat.CompleteAsync(prompt, ct);
        // Clean up the title (remove quotes if any)
        title = title.Trim('\"', '\'', ' ', '\n', '\r');
        return Ok(new { title });
    }
}

public class TitleRequest
{
    public string Question { get; set; } = "";
    public string Language { get; set; } = "bs";
}

[ApiController]
[Route("api/[controller]")]
public class HealthController : ControllerBase
{
    [HttpGet]
    public IActionResult Get() => Ok(new { status = "healthy", service = "IlmAI AI Server", timestamp = DateTime.UtcNow });
}
