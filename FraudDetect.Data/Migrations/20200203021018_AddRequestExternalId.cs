using Microsoft.EntityFrameworkCore.Migrations;

namespace FraudDetect.Data.Migrations
{
    public partial class AddRequestExternalId : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "JsonResponse",
                table: "Responses");

            migrationBuilder.AddColumn<string>(
                name: "Json",
                table: "Responses",
                maxLength: 32000,
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "Score",
                table: "Responses",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<string>(
                name: "ExternalId",
                table: "Requests",
                maxLength: 20,
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "Json",
                table: "Requests",
                maxLength: 32000,
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "ParseError",
                table: "Requests",
                maxLength: 1000,
                nullable: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Json",
                table: "Responses");

            migrationBuilder.DropColumn(
                name: "Score",
                table: "Responses");

            migrationBuilder.DropColumn(
                name: "ExternalId",
                table: "Requests");

            migrationBuilder.DropColumn(
                name: "Json",
                table: "Requests");

            migrationBuilder.DropColumn(
                name: "ParseError",
                table: "Requests");

            migrationBuilder.AddColumn<string>(
                name: "JsonResponse",
                table: "Responses",
                type: "nvarchar(max)",
                maxLength: 32000,
                nullable: true);
        }
    }
}
