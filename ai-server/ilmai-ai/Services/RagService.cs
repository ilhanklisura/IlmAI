namespace IlmAI.AI.Services;
using IlmAI.AI.Models.Request;
using IlmAI.AI.Models.Response;

public interface IRagService : IService
{
    Task<RagResponseDto> AskGlobalAsync(string question, string language, List<ChatHistoryItem>? history = null, CancellationToken ct = default);
}

public class RagService : IRagService
{
    private const int TopK = 10;
    private const double ScoreThresholdStrong = 0.55;
    private const double ScoreThresholdWeak = 0.40;
    private const int MaxContextLength = 12000;

    private readonly IEmbeddingService _embedding;
    private readonly IRetrievalService _retrieval;
    private readonly IChatCompletionService _chat;
    private readonly IPromptBuilderService _promptBuilder;
    private readonly ILogger<RagService> _logger;

    public RagService(IEmbeddingService embedding, IRetrievalService retrieval,
        IChatCompletionService chat, IPromptBuilderService promptBuilder, ILogger<RagService> logger)
    {
        _embedding = embedding; _retrieval = retrieval;
        _chat = chat; _promptBuilder = promptBuilder; _logger = logger;
    }

    public async Task<RagResponseDto> AskGlobalAsync(string question, string language, List<ChatHistoryItem>? history = null, CancellationToken ct = default)
    {
        language = LanguageDetector.Detect(question, language);
        var queryEmbedding = await _embedding.GetEmbeddingAsync(question, ct);

        var sources = await _retrieval.RetrieveAsync(question, queryEmbedding, TopK, ScoreThresholdWeak, language, ct);
        
        var topScore = sources.Count > 0 ? sources.Max(s => s.Score) : 0;
        var strongChunks = sources.Where(s => s.Score >= ScoreThresholdStrong).ToList();

        _logger.LogInformation("RAG: topScore={TopScore:F3}, strongChunks={Strong}, total={Total}",
            topScore, strongChunks.Count, sources.Count);

        var contextChunks = strongChunks.Count > 0 ? strongChunks : sources.Take(5).ToList();
        var contextText = contextChunks.Count > 0 
            ? string.Join("\n\n---\n\n", contextChunks.Select((s, i) => $"[IZVOR {i + 1}] {s.DocumentTitle}\n{s.Content}"))
            : ""; // Empty context for greetings/small talk
            
        if (contextText.Length > MaxContextLength)
            contextText = contextText[..MaxContextLength];

        var prompt = _promptBuilder.BuildRagPrompt(question, contextText, language, history);
        var answer = await _chat.CompleteAsync(prompt, ct);

        var lowConfidence = topScore < 0.85;
        var hasAnswer = !answer.Contains("nemam dovoljno informacija", StringComparison.OrdinalIgnoreCase);

        return new RagResponseDto(hasAnswer, answer, topScore, lowConfidence,
            contextChunks.Select(s => new RagSourceDto(s.DocumentTitle, s.Content, s.StartTime, s.EndTime, s.Score)).ToList());
    }
}
