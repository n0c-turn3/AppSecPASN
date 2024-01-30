using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace AppSecPASN.Migrations
{
    public partial class UpdatedPasswords : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Passwords_AspNetUsers_UserId1",
                table: "Passwords");

            migrationBuilder.DropIndex(
                name: "IX_Passwords_UserId1",
                table: "Passwords");

            migrationBuilder.DropColumn(
                name: "UserId1",
                table: "Passwords");

            migrationBuilder.AlterColumn<string>(
                name: "UserId",
                table: "Passwords",
                type: "nvarchar(450)",
                nullable: false,
                oldClrType: typeof(int),
                oldType: "int");

            migrationBuilder.CreateIndex(
                name: "IX_Passwords_UserId",
                table: "Passwords",
                column: "UserId");

            migrationBuilder.AddForeignKey(
                name: "FK_Passwords_AspNetUsers_UserId",
                table: "Passwords",
                column: "UserId",
                principalTable: "AspNetUsers",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Passwords_AspNetUsers_UserId",
                table: "Passwords");

            migrationBuilder.DropIndex(
                name: "IX_Passwords_UserId",
                table: "Passwords");

            migrationBuilder.AlterColumn<int>(
                name: "UserId",
                table: "Passwords",
                type: "int",
                nullable: false,
                oldClrType: typeof(string),
                oldType: "nvarchar(450)");

            migrationBuilder.AddColumn<string>(
                name: "UserId1",
                table: "Passwords",
                type: "nvarchar(450)",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_Passwords_UserId1",
                table: "Passwords",
                column: "UserId1");

            migrationBuilder.AddForeignKey(
                name: "FK_Passwords_AspNetUsers_UserId1",
                table: "Passwords",
                column: "UserId1",
                principalTable: "AspNetUsers",
                principalColumn: "Id");
        }
    }
}
