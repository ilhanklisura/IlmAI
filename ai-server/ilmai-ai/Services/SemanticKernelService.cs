namespace IlmAI.AI.Services;
using Microsoft.SemanticKernel;
public interface ISemanticKernelService : IService { Task<string> InvokePromptAsync(string prompt, CancellationToken ct = default); }
public class SemanticKernelService : ISemanticKernelService
{
    private readonly Kernel? _kernel;
    private readonly IChatCompletionService _fallback;
    public SemanticKernelService(IChatCompletionService fallback, Kernel? kernel = null)
    {
        _kernel = kernel; _fallback = fallback;
    }
    public async Task<string> InvokePromptAsync(string prompt, CancellationToken ct = default)
    {
        if (_kernel != null)
        {
            var result = await _kernel.InvokePromptAsync(prompt, cancellationToken: ct);
            return result?.GetValue<string>() ?? "";
        }
        return await _fallback.CompleteAsync(prompt, ct);
    }
}
