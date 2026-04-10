using FluentMigrator;

namespace IlmAI.Api.Models.Data.Migrations;

[Migration(2026041002)]
public class _2026041002_AddEmailVerificationFields : Migration
{
    public override void Up()
    {
        Alter.Table("users")
            .AddColumn("is_email_verified").AsBoolean().WithDefaultValue(false)
            .AddColumn("email_verification_code").AsString(20).Nullable();
    }

    public override void Down()
    {
        Delete.Column("email_verification_code").FromTable("users");
        Delete.Column("is_email_verified").FromTable("users");
    }
}
