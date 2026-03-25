namespace IlmAI.AI.Services;
using IlmAI.AI.Providers;
using Pgvector;
public interface IEmbeddingService : IService { Task<Vector> GetEmbeddingAsync(string text, CancellationToken ct = default); }
public class EmbeddingService : IEmbeddingService
{
    private readonly IEmbeddingProvider _provider;
    public EmbeddingService(IEmbeddingProvider provider) { _provider = provider; }
    public Task<Vector> GetEmbeddingAsync(string text, CancellationToken ct = default) => _provider.EmbedAsync(text, ct);
}
