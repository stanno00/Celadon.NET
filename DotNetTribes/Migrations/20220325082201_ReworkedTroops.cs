using Microsoft.EntityFrameworkCore.Migrations;

namespace DotNetTribes.Migrations
{
    public partial class ReworkedTroops : Migration
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

            migrationBuilder.AddColumn<int>(
                name: "Attack",
                table: "Troops",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<int>(
                name: "Capacity",
                table: "Troops",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<bool>(
                name: "ConsumingFood",
                table: "Troops",
                type: "bit",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<int>(
                name: "Defense",
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

            migrationBuilder.AddColumn<int>(
                name: "Level",
                table: "Troops",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<long>(
                name: "StartedAt",
                table: "Troops",
                type: "bigint",
                nullable: false,
                defaultValue: 0L);

            migrationBuilder.AddColumn<long>(
                name: "UpdatedAt",
                table: "Troops",
                type: "bigint",
                nullable: false,
                defaultValue: 0L);

            migrationBuilder.AddColumn<int>(
                name: "StorageLimit",
                table: "GameRules",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<int>(
                name: "TroopAttack",
                table: "GameRules",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<int>(
                name: "TroopCapacity",
                table: "GameRules",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<int>(
                name: "TroopDefense",
                table: "GameRules",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<int>(
                name: "TroopFoodConsumption",
                table: "GameRules",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.UpdateData(
                table: "GameRules",
                keyColumn: "GameRulesId",
                keyValue: 1,
                columns: new[] { "StorageLimit", "TroopAttack", "TroopCapacity", "TroopDefense", "TroopFoodConsumption" },
                values: new object[] { 100, 10, 2, 5, 2 });

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
                name: "Attack",
                table: "Troops");

            migrationBuilder.DropColumn(
                name: "Capacity",
                table: "Troops");

            migrationBuilder.DropColumn(
                name: "ConsumingFood",
                table: "Troops");

            migrationBuilder.DropColumn(
                name: "Defense",
                table: "Troops");

            migrationBuilder.DropColumn(
                name: "FinishedAt",
                table: "Troops");

            migrationBuilder.DropColumn(
                name: "Level",
                table: "Troops");

            migrationBuilder.DropColumn(
                name: "StartedAt",
                table: "Troops");

            migrationBuilder.DropColumn(
                name: "UpdatedAt",
                table: "Troops");

            migrationBuilder.DropColumn(
                name: "StorageLimit",
                table: "GameRules");

            migrationBuilder.DropColumn(
                name: "TroopAttack",
                table: "GameRules");

            migrationBuilder.DropColumn(
                name: "TroopCapacity",
                table: "GameRules");

            migrationBuilder.DropColumn(
                name: "TroopDefense",
                table: "GameRules");

            migrationBuilder.DropColumn(
                name: "TroopFoodConsumption",
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
