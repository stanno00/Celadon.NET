using Microsoft.EntityFrameworkCore.Migrations;

namespace EntityFrameworkMatReview.Migrations
{
    public partial class pet : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "PetId",
                table: "Students",
                nullable: true);

            migrationBuilder.CreateTable(
                name: "Pet",
                columns: table => new
                {
                    PetId = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Name = table.Column<string>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Pet", x => x.PetId);
                });

            migrationBuilder.CreateIndex(
                name: "IX_Students_PetId",
                table: "Students",
                column: "PetId");

            migrationBuilder.AddForeignKey(
                name: "FK_Students_Pet_PetId",
                table: "Students",
                column: "PetId",
                principalTable: "Pet",
                principalColumn: "PetId",
                onDelete: ReferentialAction.Restrict);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Students_Pet_PetId",
                table: "Students");

            migrationBuilder.DropTable(
                name: "Pet");

            migrationBuilder.DropIndex(
                name: "IX_Students_PetId",
                table: "Students");

            migrationBuilder.DropColumn(
                name: "PetId",
                table: "Students");
        }
    }
}
