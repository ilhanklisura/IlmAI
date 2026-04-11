using FluentMigrator;

namespace IlmAI.Api.Models.Data.Migrations;

[Migration(2026041101)]
public class _2026041101_AddUserLastActiveAt : Migration
{
    public override void Up()
    {
        if (!Schema.Table("users").Column("last_active_at").Exists())
        {
            Alter.Table("users")
                .AddColumn("last_active_at").AsDateTime().Nullable();
        }
    }

    public override void Down()
    {
        if (Schema.Table("users").Column("last_active_at").Exists())
        {
            Delete.Column("last_active_at").FromTable("users");
        }
    }
}
