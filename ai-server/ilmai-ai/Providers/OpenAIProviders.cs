namespace IlmAI.AI.Providers;
using System.Net.Http.Json;
using System.Text.Json;
using System.Text.Json.Serialization;
using IlmAI.AI.Models.Option;
using IlmAI.AI.Services;
using Microsoft.Extensions.Options;
using Pgvector;

public class OpenAIChatProvider : IChatProvider, IService
{
    private readonly IHttpClientFactory _httpClientFactory;
    private readonly OpenAIOptions _options;
    public OpenAIChatProvider(IHttpClientFactory httpClientFactory, IOptions<OpenAIOptions> options)
    {
        _httpClientFactory = httpClientFactory;
        _options = options.Value;
    }
    public async Task<string> CompleteAsync(string prompt, CancellationToken ct = default)
    {
        var client = _httpClientFactory.CreateClient("OpenAI");
        var request = new
        {
            model = _options.ChatModel,
            messages = new[] { new { role = "user", content = prompt } },
            max_tokens = 2000,
            temperature = 0.3
        };
        var response = await client.PostAsJsonAsync("chat/completions", request, ct);
        if (!response.IsSuccessStatusCode)
        {
            var error = await response.Content.ReadAsStringAsync(ct);
            Console.WriteLine($"❌ OpenAI ERROR: {response.StatusCode} - {error}");
            throw new Exception($"OpenAI failed: {error}");
        }
        response.EnsureSuccessStatusCode();
        var json = await response.Content.ReadFromJsonAsync<JsonElement>(ct);
        return json.GetProperty("choices")[0].GetProperty("message").GetProperty("content").GetString() ?? "";
    }
}

public class OpenAIEmbeddingProvider : IEmbeddingProvider, IService
{
    private readonly IHttpClientFactory _httpClientFactory;
    private readonly OpenAIOptions _options;
    public OpenAIEmbeddingProvider(IHttpClientFactory httpClientFactory, IOptions<OpenAIOptions> options)
    {
        _httpClientFactory = httpClientFactory;
        _options = options.Value;
    }
    public async Task<Vector> EmbedAsync(string text, CancellationToken ct = default)
    {
        var client = _httpClientFactory.CreateClient("OpenAI");
        var request = new { model = _options.EmbeddingModel, input = text, dimensions = _options.EmbeddingDimensions };
        var response = await client.PostAsJsonAsync("embeddings", request, ct);
        response.EnsureSuccessStatusCode();
        var json = await response.Content.ReadFromJsonAsync<JsonElement>(ct);
        var embeddingArray = json.GetProperty("data")[0].GetProperty("embedding");
        var floats = new float[_options.EmbeddingDimensions];
        int i = 0;
        foreach (var elem in embeddingArray.EnumerateArray())
            floats[i++] = elem.GetSingle();
        return new Vector(floats);
    }
}
