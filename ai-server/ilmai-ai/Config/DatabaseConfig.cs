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
                options.UseNpgsql(connectionString, npgsql => npgsql.UseVector()).UseSnakeCaseNamingConvention())
            .AddFluentMigratorCore()
            .ConfigureRunner(rb => rb
                .AddPostgres()
                .WithGlobalConnectionString(connectionString)
                .ScanIn(typeof(DatabaseConfig).Assembly).For.Migrations());
    }
    public void ConfigureApp(IApplicationBuilder app, IWebHostEnvironment env, IConfiguration config)
    {
        // Ensure vector extension exists at startup
        using var scope = app.ApplicationServices.CreateScope();
        var context = scope.ServiceProvider.GetRequiredService<AIDataContext>();
        try 
        {
            context.Database.OpenConnection();
            using var command = context.Database.GetDbConnection().CreateCommand();
            command.CommandText = "CREATE EXTENSION IF NOT EXISTS vector;";
            command.ExecuteNonQuery();
            
            // Reload types to ensure Npgsql recognizes 'vector' if it was just created
            if (context.Database.GetDbConnection() is Npgsql.NpgsqlConnection npgsqlConn)
            {
                npgsqlConn.ReloadTypes();
            }
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Warning: Could not ensure vector extension: {ex.Message}");
        }
        finally
        {
            context.Database.CloseConnection();
        }
    }
}
