namespace IlmAI.AI.Config;
using IlmAI.AI.Models.Option;
using Microsoft.SemanticKernel;
public class SemanticKernelConfig : IConfig
{
    public void ConfigureServices(IServiceCollection services, IConfiguration config)
    {
        var openAiOptions = config.GetSection("OpenAI").Get<OpenAIOptions>();
        if (openAiOptions == null || string.IsNullOrEmpty(openAiOptions.ApiKey)) return;

        var kernelBuilder = Kernel.CreateBuilder();
        kernelBuilder.AddOpenAIChatCompletion(openAiOptions.ChatModel, openAiOptions.ApiKey);
        var kernel = kernelBuilder.Build();
        services.AddSingleton(kernel);
    }
}
