namespace IlmAI.Api.Models.Data.Migrations;

using FluentMigrator;

[Migration(2026032501)]
public class _2026032501_InitialSchema : Migration
{
    public override void Up()
    {
        // Enable uuid-ossp for uuid_generate_v4()
        Execute.Sql("CREATE EXTENSION IF NOT EXISTS \"uuid-ossp\";");

        // Users
        Create.Table("users")
            .WithColumn("id").AsGuid().PrimaryKey().WithDefault(SystemMethods.NewGuid)
            .WithColumn("username").AsString(100).NotNullable().Unique()
            .WithColumn("email").AsString(255).NotNullable().Unique()
            .WithColumn("password_hash").AsString(255).NotNullable()
            .WithColumn("first_name").AsString(100).Nullable()
            .WithColumn("last_name").AsString(100).Nullable()
            .WithColumn("preferred_language").AsString(10).WithDefaultValue("bs")
            .WithColumn("is_active").AsBoolean().WithDefaultValue(true)
            .WithColumn("is_deleted").AsBoolean().WithDefaultValue(false)
            .WithColumn("created_at").AsDateTimeOffset().WithDefault(SystemMethods.CurrentUTCDateTime)
            .WithColumn("updated_at").AsDateTimeOffset().WithDefault(SystemMethods.CurrentUTCDateTime);

        // Roles
        Create.Table("roles")
            .WithColumn("id").AsGuid().PrimaryKey().WithDefault(SystemMethods.NewGuid)
            .WithColumn("name").AsString(50).NotNullable().Unique()
            .WithColumn("description").AsString(255).Nullable()
            .WithColumn("created_at").AsDateTimeOffset().WithDefault(SystemMethods.CurrentUTCDateTime);

        // User Roles
        Create.Table("user_roles")
            .WithColumn("user_id").AsGuid().NotNullable().ForeignKey("users", "id")
            .WithColumn("role_id").AsGuid().NotNullable().ForeignKey("roles", "id");

        Create.PrimaryKey("pk_user_roles").OnTable("user_roles").Columns("user_id", "role_id");

        // User Settings
        Create.Table("user_settings")
            .WithColumn("id").AsGuid().PrimaryKey().WithDefault(SystemMethods.NewGuid)
            .WithColumn("user_id").AsGuid().NotNullable().ForeignKey("users", "id")
            .WithColumn("theme").AsString(20).WithDefaultValue("dark")
            .WithColumn("language").AsString(10).WithDefaultValue("bs")
            .WithColumn("notifications_enabled").AsBoolean().WithDefaultValue(true)
            .WithColumn("created_at").AsDateTimeOffset().WithDefault(SystemMethods.CurrentUTCDateTime)
            .WithColumn("updated_at").AsDateTimeOffset().WithDefault(SystemMethods.CurrentUTCDateTime);

        // Chat Sessions
        Create.Table("chat_sessions")
            .WithColumn("id").AsGuid().PrimaryKey().WithDefault(SystemMethods.NewGuid)
            .WithColumn("user_id").AsGuid().NotNullable().ForeignKey("users", "id")
            .WithColumn("title").AsString(255).Nullable()
            .WithColumn("created_at").AsDateTimeOffset().WithDefault(SystemMethods.CurrentUTCDateTime)
            .WithColumn("updated_at").AsDateTimeOffset().WithDefault(SystemMethods.CurrentUTCDateTime);

        // Chat Messages
        Create.Table("chat_messages")
            .WithColumn("id").AsGuid().PrimaryKey().WithDefault(SystemMethods.NewGuid)
            .WithColumn("session_id").AsGuid().NotNullable().ForeignKey("chat_sessions", "id")
            .WithColumn("role").AsString(20).NotNullable()
            .WithColumn("content").AsCustom("TEXT").NotNullable()
            .WithColumn("sources_json").AsCustom("TEXT").Nullable()
            .WithColumn("created_at").AsDateTimeOffset().WithDefault(SystemMethods.CurrentUTCDateTime);

        // Favorite Items
        Create.Table("favorite_items")
            .WithColumn("id").AsGuid().PrimaryKey().WithDefault(SystemMethods.NewGuid)
            .WithColumn("user_id").AsGuid().NotNullable().ForeignKey("users", "id")
            .WithColumn("item_type").AsString(50).NotNullable()
            .WithColumn("item_id").AsString(100).NotNullable()
            .WithColumn("note").AsCustom("TEXT").Nullable()
            .WithColumn("created_at").AsDateTimeOffset().WithDefault(SystemMethods.CurrentUTCDateTime);

        // Search History
        Create.Table("search_history")
            .WithColumn("id").AsGuid().PrimaryKey().WithDefault(SystemMethods.NewGuid)
            .WithColumn("user_id").AsGuid().NotNullable().ForeignKey("users", "id")
            .WithColumn("query").AsCustom("TEXT").NotNullable()
            .WithColumn("results_count").AsInt32().WithDefaultValue(0)
            .WithColumn("created_at").AsDateTimeOffset().WithDefault(SystemMethods.CurrentUTCDateTime);

        // Seed default roles
        Insert.IntoTable("roles").Row(new { id = Guid.NewGuid(), name = "admin", description = "Administrator" });
        Insert.IntoTable("roles").Row(new { id = Guid.NewGuid(), name = "user", description = "Regular user" });
    }

    public override void Down()
    {
        Delete.Table("search_history");
        Delete.Table("favorite_items");
        Delete.Table("chat_messages");
        Delete.Table("chat_sessions");
        Delete.Table("user_settings");
        Delete.Table("user_roles");
        Delete.Table("roles");
        Delete.Table("users");
    }
}
