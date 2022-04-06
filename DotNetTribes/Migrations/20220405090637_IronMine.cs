using Microsoft.EntityFrameworkCore.Migrations;

namespace DotNetTribes.Migrations
{
    public partial class IronMine : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "IronMineCost",
                table: "GameRules",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<int>(
                name: "IronMineDuration",
                table: "GameRules",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<int>(
                name: "IronMineGeneration",
                table: "GameRules",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<int>(
                name: "IronMineHP",
                table: "GameRules",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.UpdateData(
                table: "GameRules",
                keyColumn: "GameRulesId",
                keyValue: 1,
                columns: new[] { "IronMineCost", "IronMineDuration", "IronMineGeneration", "IronMineHP" },
                values: new object[] { 100, 60, 5, 100 });
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "IronMineCost",
                table: "GameRules");

            migrationBuilder.DropColumn(
                name: "IronMineDuration",
                table: "GameRules");

            migrationBuilder.DropColumn(
                name: "IronMineGeneration",
                table: "GameRules");

            migrationBuilder.DropColumn(
                name: "IronMineHP",
                table: "GameRules");
        }
    }
}
