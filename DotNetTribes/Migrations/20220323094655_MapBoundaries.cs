using Microsoft.EntityFrameworkCore.Migrations;

namespace DotNetTribes.Migrations
{
    public partial class MapBoundaries : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "MapBoundariesX",
                table: "GameRules",
                type: "int",
                nullable: false,
                defaultValue: 101);

            migrationBuilder.AddColumn<int>(
                name: "MapBoundariesY",
                table: "GameRules",
                type: "int",
                nullable: false,
                defaultValue: 101);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "MapBoundariesX",
                table: "GameRules",
                type: "int",
                nullable: false,
                defaultValue: 101);

            migrationBuilder.AddColumn<int>(
                name: "MapBoundariesY",
                table: "GameRules",
                type: "int",
                nullable: false,
                defaultValue: 101);
        }
    }
}