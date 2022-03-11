using Microsoft.EntityFrameworkCore.Migrations;

namespace DotNetTribes.Migrations
{
    public partial class updatedResource : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Resources_Kingdoms_KingdomId",
                table: "Resources");

            migrationBuilder.AlterColumn<int>(
                name: "KingdomId",
                table: "Resources",
                type: "int",
                nullable: false,
                defaultValue: 0,
                oldClrType: typeof(int),
                oldType: "int",
                oldNullable: true);

            migrationBuilder.AddForeignKey(
                name: "FK_Resources_Kingdoms_KingdomId",
                table: "Resources",
                column: "KingdomId",
                principalTable: "Kingdoms",
                principalColumn: "KingdomId",
                onDelete: ReferentialAction.Cascade);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Resources_Kingdoms_KingdomId",
                table: "Resources");

            migrationBuilder.AlterColumn<int>(
                name: "KingdomId",
                table: "Resources",
                type: "int",
                nullable: true,
                oldClrType: typeof(int),
                oldType: "int");

            migrationBuilder.AddForeignKey(
                name: "FK_Resources_Kingdoms_KingdomId",
                table: "Resources",
                column: "KingdomId",
                principalTable: "Kingdoms",
                principalColumn: "KingdomId",
                onDelete: ReferentialAction.Restrict);
        }
    }
}
