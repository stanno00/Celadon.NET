using Microsoft.EntityFrameworkCore.Migrations;

namespace DotNetTribes.Migrations
{
    public partial class Coordinates : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "KingdomX",
                table: "GameRules",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<int>(
                name: "KingdomY",
                table: "GameRules",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.UpdateData(
                table: "GameRules",
                keyColumn: "GameRulesId",
                keyValue: 1,
                columns: new[] { "KingdomX", "KingdomY" },
                values: new object[] { 101, 101 });
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "KingdomX",
                table: "GameRules");

            migrationBuilder.DropColumn(
                name: "KingdomY",
                table: "GameRules");
        }
    }
}
