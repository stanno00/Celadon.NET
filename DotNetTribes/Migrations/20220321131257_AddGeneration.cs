using Microsoft.EntityFrameworkCore.Migrations;

namespace DotNetTribes.Migrations
{
    public partial class AddGeneration : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Troops_Kingdoms_KingdomId",
                table: "Troops");

            migrationBuilder.AlterColumn<int>(
                name: "KingdomId",
                table: "Troops",
                type: "int",
                nullable: false,
                defaultValue: 0,
                oldClrType: typeof(int),
                oldType: "int",
                oldNullable: true);

            migrationBuilder.AddColumn<bool>(
                name: "ConsumingFood",
                table: "Troops",
                type: "bit",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<int>(
                name: "CoordinateX",
                table: "Troops",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<int>(
                name: "CoordinateY",
                table: "Troops",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<long>(
                name: "FinishedAt",
                table: "Troops",
                type: "bigint",
                nullable: false,
                defaultValue: 0L);

            migrationBuilder.AddColumn<long>(
                name: "StartedAt",
                table: "Troops",
                type: "bigint",
                nullable: false,
                defaultValue: 0L);

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

            migrationBuilder.AddForeignKey(
                name: "FK_Troops_Kingdoms_KingdomId",
                table: "Troops",
                column: "KingdomId",
                principalTable: "Kingdoms",
                principalColumn: "KingdomId",
                onDelete: ReferentialAction.Cascade);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Troops_Kingdoms_KingdomId",
                table: "Troops");

            migrationBuilder.DropColumn(
                name: "ConsumingFood",
                table: "Troops");

            migrationBuilder.DropColumn(
                name: "CoordinateX",
                table: "Troops");

            migrationBuilder.DropColumn(
                name: "CoordinateY",
                table: "Troops");

            migrationBuilder.DropColumn(
                name: "FinishedAt",
                table: "Troops");

            migrationBuilder.DropColumn(
                name: "StartedAt",
                table: "Troops");

            migrationBuilder.DropColumn(
                name: "FarmAllLevelsFoodGeneration",
                table: "GameRules");

            migrationBuilder.DropColumn(
                name: "MineALlLevelsGoldGeneration",
                table: "GameRules");

            migrationBuilder.AlterColumn<int>(
                name: "KingdomId",
                table: "Troops",
                type: "int",
                nullable: true,
                oldClrType: typeof(int),
                oldType: "int");

            migrationBuilder.AddForeignKey(
                name: "FK_Troops_Kingdoms_KingdomId",
                table: "Troops",
                column: "KingdomId",
                principalTable: "Kingdoms",
                principalColumn: "KingdomId",
                onDelete: ReferentialAction.Restrict);
        }
    }
}
