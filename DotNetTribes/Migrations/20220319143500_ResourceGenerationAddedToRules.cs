using Microsoft.EntityFrameworkCore.Migrations;

namespace DotNetTribes.Migrations
{
    public partial class ResourceGenerationAddedToRules : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "FarmAllLevelsFoodGeneration",
                table: "GameRules",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<int>(
                name: "MineALlLevelsGoldGeneration",
                table: "GameRules",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.UpdateData(
                table: "GameRules",
                keyColumn: "GameRulesId",
                keyValue: 1,
                columns: new[] { "FarmAllLevelsFoodGeneration", "MineALlLevelsGoldGeneration" },
                values: new object[] { 5, 5 });
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "FarmAllLevelsFoodGeneration",
                table: "GameRules");

            migrationBuilder.DropColumn(
                name: "MineALlLevelsGoldGeneration",
                table: "GameRules");
        }
    }
}
