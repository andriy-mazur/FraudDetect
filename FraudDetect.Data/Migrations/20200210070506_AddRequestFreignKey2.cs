using Microsoft.EntityFrameworkCore.Migrations;

namespace FraudDetect.Data.Migrations
{
    public partial class AddRequestFreignKey2 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Responses_Requests_Request_Id",
                table: "Responses");

            migrationBuilder.AlterColumn<int>(
                name: "Request_Id",
                table: "Responses",
                nullable: false,
                oldClrType: typeof(int),
                oldType: "int",
                oldNullable: true);

            migrationBuilder.AddForeignKey(
                name: "FK_Responses_Requests_Request_Id",
                table: "Responses",
                column: "Request_Id",
                principalTable: "Requests",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Responses_Requests_Request_Id",
                table: "Responses");

            migrationBuilder.AlterColumn<int>(
                name: "Request_Id",
                table: "Responses",
                type: "int",
                nullable: true,
                oldClrType: typeof(int));

            migrationBuilder.AddForeignKey(
                name: "FK_Responses_Requests_Request_Id",
                table: "Responses",
                column: "Request_Id",
                principalTable: "Requests",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }
    }
}
