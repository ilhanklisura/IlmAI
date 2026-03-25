namespace IlmAI.AI.Config;
using FluentMigrator.Runner;
using IlmAI.AI.Data;
using Microsoft.EntityFrameworkCore;
using Pgvector.EntityFrameworkCore;
public class DatabaseConfig : IConfig
{
    public void ConfigureServices(IServiceCollection services, IConfiguration config)
    {
        var connectionString = config.GetConnectionString("Default")
            ?? throw new InvalidOperationException("Missing ConnectionStrings:Default");
        services
            .AddDbContext<AIDataContext>(options =>
                options.UseNpgsql(connectionString, npgsql => npgsql.UseVector()))
            .AddFluentMigratorCore()
            .ConfigureRunner(rb => rb
                .AddPostgres()
                .WithGlobalConnectionString(connectionString)
                .ScanIn(typeof(DatabaseConfig).Assembly).For.Migrations());
    }
    public void ConfigureApp(IApplicationBuilder app, IWebHostEnvironment env, IConfiguration config)
    {
        // Migrations are handled by the backend API service
    }
}
