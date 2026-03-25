namespace IlmAI.AI.Config;
using IlmAI.AI.Models.Option;
public class OpenAIConfig : IConfig
{
    public void ConfigureServices(IServiceCollection services, IConfiguration config)
    {
        var openAiOptions = config.GetSection("OpenAI").Get<OpenAIOptions>()
            ?? throw new InvalidOperationException("Missing OpenAI configuration");
        services.AddHttpClient("OpenAI", client =>
        {
            client.BaseAddress = new Uri("https://api.openai.com/v1/");
            client.DefaultRequestHeaders.Add("Authorization", $"Bearer {openAiOptions.ApiKey}");
            client.Timeout = TimeSpan.FromSeconds(120);
        });
    }
}
