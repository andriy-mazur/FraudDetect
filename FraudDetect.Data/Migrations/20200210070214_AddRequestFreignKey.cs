using Microsoft.EntityFrameworkCore.Migrations;

namespace FraudDetect.Data.Migrations
{
    public partial class AddRequestFreignKey : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Responses_Requests_RequestId",
                table: "Responses");

            migrationBuilder.DropIndex(
                name: "IX_Responses_RequestId",
                table: "Responses");

            migrationBuilder.DropColumn(
                name: "RequestId",
                table: "Responses");

            migrationBuilder.AddColumn<int>(
                name: "Request_Id",
                table: "Responses",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_Responses_Request_Id",
                table: "Responses",
                column: "Request_Id");

            migrationBuilder.AddForeignKey(
                name: "FK_Responses_Requests_Request_Id",
                table: "Responses",
                column: "Request_Id",
                principalTable: "Requests",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Responses_Requests_Request_Id",
                table: "Responses");

            migrationBuilder.DropIndex(
                name: "IX_Responses_Request_Id",
                table: "Responses");

            migrationBuilder.DropColumn(
                name: "Request_Id",
                table: "Responses");

            migrationBuilder.AddColumn<int>(
                name: "RequestId",
                table: "Responses",
                type: "int",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_Responses_RequestId",
                table: "Responses",
                column: "RequestId");

            migrationBuilder.AddForeignKey(
                name: "FK_Responses_Requests_RequestId",
                table: "Responses",
                column: "RequestId",
                principalTable: "Requests",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }
    }
}
