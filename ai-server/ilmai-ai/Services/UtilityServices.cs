namespace IlmAI.AI.Services;
public interface IChunkingService : IService { List<string> ChunkText(string text, int chunkSize = 500, int overlap = 50); }
public class ChunkingService : IChunkingService
{
    public List<string> ChunkText(string text, int chunkSize = 500, int overlap = 50)
    {
        var words = text.Split(' ', StringSplitOptions.RemoveEmptyEntries);
        var chunks = new List<string>();
        int start = 0;
        while (start < words.Length)
        {
            var end = Math.Min(start + chunkSize, words.Length);
            chunks.Add(string.Join(' ', words[start..end]));
            start += chunkSize - overlap;
        }
        return chunks;
    }
}
public interface IFileParserService : IService { Task<string> ParseAsync(Stream stream, string fileName); }
public class FileParserService : IFileParserService
{
    public async Task<string> ParseAsync(Stream stream, string fileName)
    {
        using var reader = new StreamReader(stream);
        return await reader.ReadToEndAsync();
    }
}
public interface ISearchService : IService
{
    Task<List<RetrievedChunk>> SearchAsync(string question, string language, int topK, CancellationToken ct = default);
}
public class SearchService : ISearchService
{
    private readonly IEmbeddingService _embedding;
    private readonly IRetrievalService _retrieval;
    public SearchService(IEmbeddingService embedding, IRetrievalService retrieval)
    {
        _embedding = embedding; _retrieval = retrieval;
    }
    public async Task<List<RetrievedChunk>> SearchAsync(string question, string language, int topK, CancellationToken ct = default)
    {
        language = LanguageDetector.Detect(question, language);
        var embedding = await _embedding.GetEmbeddingAsync(question, ct);
        return await _retrieval.RetrieveAsync(embedding, topK, 0.40, language, ct);
    }
}

public static class LanguageDetector
{
    private static readonly HashSet<string> EnglishWords = new(StringComparer.OrdinalIgnoreCase) { "the", "what", "is", "how", "why", "who", "when", "where", "are", "do", "does", "can", "will", "and", "or", "in", "on", "at", "to", "for" };
    private static readonly HashSet<string> BosnianWords = new(StringComparer.OrdinalIgnoreCase) { "šta", "kako", "zašto", "ko", "gdje", "kada", "je", "su", "da", "li", "će", "ću", "bi", "i", "ili", "u", "na", "za", "o" };

    public static string Detect(string text, string defaultLanguage = "bs")
    {
        var words = text.Split(new[] { ' ', ',', '.', '?', '!', '\n', '\r' }, StringSplitOptions.RemoveEmptyEntries);
        int enCount = words.Count(w => EnglishWords.Contains(w));
        int bsCount = words.Count(w => BosnianWords.Contains(w));
        if (enCount > bsCount) return "en";
        if (bsCount > enCount) return "bs";
        return defaultLanguage;
    }
}
