namespace IlmAI.AI.Providers;
using Pgvector;
public interface IChatProvider 
{ 
    Task<string> CompleteAsync(string prompt, CancellationToken ct = default); 
}
public interface IEmbeddingProvider 
{ 
    Task<Vector> EmbedAsync(string text, CancellationToken ct = default); 
}
