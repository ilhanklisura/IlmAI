namespace IlmAI.AI.Models.Option;
public class AppOptions
{
    public string? ConnectionStrings { get; set; }
}
public class OpenAIOptions
{
    public string ApiKey { get; set; } = "";
    public string ChatModel { get; set; } = "gpt-4o-mini";
    public string EmbeddingModel { get; set; } = "text-embedding-3-small";
    public int EmbeddingDimensions { get; set; } = 1536;
}
