using Microsoft.EntityFrameworkCore.Migrations;

namespace DotNetTribes.Migrations
{
    public partial class SecurityQuestion : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "SecurityQuestionId",
                table: "Users",
                type: "int",
                nullable: true);

            migrationBuilder.CreateTable(
                name: "SecurityQuestions",
                columns: table => new
                {
                    SecurityQuestionId = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    TheQuestion = table.Column<string>(type: "nvarchar(20)", nullable: false),
                    Answer = table.Column<string>(type: "nvarchar(max)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_SecurityQuestions", x => x.SecurityQuestionId);
                });

            migrationBuilder.CreateIndex(
                name: "IX_Users_SecurityQuestionId",
                table: "Users",
                column: "SecurityQuestionId",
                unique: true,
                filter: "[SecurityQuestionId] IS NOT NULL");

            migrationBuilder.AddForeignKey(
                name: "FK_Users_SecurityQuestions_SecurityQuestionId",
                table: "Users",
                column: "SecurityQuestionId",
                principalTable: "SecurityQuestions",
                principalColumn: "SecurityQuestionId",
                onDelete: ReferentialAction.Restrict);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Users_SecurityQuestions_SecurityQuestionId",
                table: "Users");

            migrationBuilder.DropTable(
                name: "SecurityQuestions");

            migrationBuilder.DropIndex(
                name: "IX_Users_SecurityQuestionId",
                table: "Users");

            migrationBuilder.DropColumn(
                name: "SecurityQuestionId",
                table: "Users");
        }
    }
}
