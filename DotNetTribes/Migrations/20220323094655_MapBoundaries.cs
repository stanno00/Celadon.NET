using Microsoft.EntityFrameworkCore.Migrations;

namespace DotNetTribes.Migrations
{
    public partial class MapBoundaries : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "KingdomY",
                table: "GameRules",
                newName: "MapBoundariesY");

            migrationBuilder.RenameColumn(
                name: "KingdomX",
                table: "GameRules",
                newName: "MapBoundariesX");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "MapBoundariesY",
                table: "GameRules",
                newName: "KingdomY");

            migrationBuilder.RenameColumn(
                name: "MapBoundariesX",
                table: "GameRules",
                newName: "KingdomX");
        }
    }
}
