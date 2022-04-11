using Microsoft.EntityFrameworkCore.Migrations;

namespace DotNetTribes.Migrations
{
    public partial class UpdateBattles : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "BattleId",
                table: "Troops",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<long>(
                name: "ReturnedFromBattleAt",
                table: "Troops",
                type: "bigint",
                nullable: false,
                defaultValue: 0L);

            migrationBuilder.AddColumn<int>(
                name: "TroopHP",
                table: "Troops",
                type: "int",
                nullable: false,
                defaultValue: 0);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "BattleId",
                table: "Troops");

            migrationBuilder.DropColumn(
                name: "ReturnedFromBattleAt",
                table: "Troops");

            migrationBuilder.DropColumn(
                name: "TroopHP",
                table: "Troops");
        }
    }
}
