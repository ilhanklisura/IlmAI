namespace IlmAI.AI.Services;
using IlmAI.AI.Providers;
public interface IChatCompletionService : IService { Task<string> CompleteAsync(string prompt, CancellationToken ct = default); }
public class ChatCompletionService : IChatCompletionService
{
    private readonly IChatProvider _provider;
    public ChatCompletionService(IChatProvider provider) { _provider = provider; }
    public Task<string> CompleteAsync(string prompt, CancellationToken ct = default) => _provider.CompleteAsync(prompt, ct);
}
