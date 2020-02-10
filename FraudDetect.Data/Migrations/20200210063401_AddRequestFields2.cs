using Microsoft.EntityFrameworkCore.Migrations;

namespace FraudDetect.Data.Migrations
{
    public partial class AddRequestFields2 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "BureauType",
                table: "Responses",
                maxLength: 50,
                nullable: true);

            migrationBuilder.AddColumn<bool>(
                name: "IsSuccess",
                table: "Responses",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<string>(
                name: "Client",
                table: "Requests",
                maxLength: 40,
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "PostalCode",
                table: "Requests",
                maxLength: 20,
                nullable: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "BureauType",
                table: "Responses");

            migrationBuilder.DropColumn(
                name: "IsSuccess",
                table: "Responses");

            migrationBuilder.DropColumn(
                name: "Client",
                table: "Requests");

            migrationBuilder.DropColumn(
                name: "PostalCode",
                table: "Requests");
        }
    }
}
