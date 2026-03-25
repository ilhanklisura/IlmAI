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
    Task<List<RetrievedChunk>> SearchAsync(string question, int topK, CancellationToken ct = default);
}
public class SearchService : ISearchService
{
    private readonly IEmbeddingService _embedding;
    private readonly IRetrievalService _retrieval;
    public SearchService(IEmbeddingService embedding, IRetrievalService retrieval)
    {
        _embedding = embedding; _retrieval = retrieval;
    }
    public async Task<List<RetrievedChunk>> SearchAsync(string question, int topK, CancellationToken ct = default)
    {
        var embedding = await _embedding.GetEmbeddingAsync(question, ct);
        return await _retrieval.RetrieveAsync(embedding, topK, 0.5, ct);
    }
}
