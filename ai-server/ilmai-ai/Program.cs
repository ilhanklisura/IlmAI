using IlmAI.AI.Config;

Console.WriteLine("IlmAI AI Server - Configuring...");

var app = WebApplication.CreateBuilder(args)
    .AddServiceConfig<OptionConfig>()
    .AddServiceConfig<DatabaseConfig>()
    .AddServiceConfig<OpenAIConfig>()
    .AddServiceConfig<SemanticKernelConfig>()
    .AddServiceConfig<MvcConfig>()
    .AddServiceConfig<SwaggerConfig>()
    .AddServiceConfig<DependencyConfig>()
    .Build();

Console.WriteLine("IlmAI AI Server - Configured, Running...");

app
    .AddAppConfig<DatabaseConfig>()
    .AddAppConfig<MvcConfig>()
    .AddAppConfig<SwaggerConfig>()
    .Run();
