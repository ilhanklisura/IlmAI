namespace IlmAI.Api.Models.Data.Migrations;

using FluentMigrator;

[Migration(2026041301)]
public class _2026041301_AddVectorExtension : Migration
{
    public override void Up()
    {
        // Enable pgvector extension
        Execute.Sql("CREATE EXTENSION IF NOT EXISTS vector;");
    }

    public override void Down()
    {
        // We generally don't drop the extension in down migrations 
        // if other tables might depend on it, but for completeness:
        // Execute.Sql("DROP EXTENSION IF EXISTS vector;");
    }
}
