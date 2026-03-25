using IlmAI.Api.Config;
using IlmAI.Api.Services.Default;

Console.WriteLine("IlmAI API - Configuring...");

var app = WebApplication.CreateBuilder(args)
    .AddServiceConfig<OptionConfig>()
    .AddServiceConfig<DatabaseConfig>()
    .AddServiceConfig<MvcConfig>()
    .AddServiceConfig<SwaggerConfig>()
    .AddServiceConfig<AuthConfig>()
    .AddServiceConfig<DependencyConfig>()
    .Build();

Console.WriteLine("IlmAI API - Configured, seeding data...");

app
    .AddAppConfig<DatabaseConfig>()
    .AddAppConfig<MvcConfig>()
    .AddAppConfig<SwaggerConfig>()
    .AddAppConfig<AuthConfig>();

// Seed default users (admin + test)
await SeedService.SeedUsersAsync(app.Services);

Console.WriteLine("IlmAI API - Running on http://localhost:5030");
app.Run();

