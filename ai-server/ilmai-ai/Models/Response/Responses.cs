namespace IlmAI.AI.Models.Response;
public record RagResponseDto(bool HasAnswer, string Answer, double ConfidenceScore, bool LowConfidence, IReadOnlyList<RagSourceDto> Sources);
public record RagSourceDto(string DocumentTitle, string Content, string? StartTime, string? EndTime, double Score);
public record SearchResultDto(string Content, string DocumentTitle, double Score, string? Metadata);
public record IndexStatusDto(Guid DocumentId, string Status, int ChunksCount);
