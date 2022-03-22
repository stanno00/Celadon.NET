using Microsoft.EntityFrameworkCore.Migrations;

namespace DotNetTribes.Migrations
{
    public partial class TroopStats : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<long>(
                name: "UpdatedAt",
                table: "Troops",
                type: "bigint",
                nullable: false,
                oldClrType: typeof(int),
                oldType: "int");

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

            migrationBuilder.AddColumn<int>(
                name: "Defense",
                table: "Troops",
                type: "int",
                nullable: false,
                defaultValue: 0);

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

            migrationBuilder.UpdateData(
                table: "GameRules",
                keyColumn: "GameRulesId",
                keyValue: 1,
                columns: new[] { "StorageLimit", "TroopAttack", "TroopCapacity", "TroopDefense" },
                values: new object[] { 100, 10, 2, 5 });
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Attack",
                table: "Troops");

            migrationBuilder.DropColumn(
                name: "Capacity",
                table: "Troops");

            migrationBuilder.DropColumn(
                name: "Defense",
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

            migrationBuilder.AlterColumn<int>(
                name: "UpdatedAt",
                table: "Troops",
                type: "int",
                nullable: false,
                oldClrType: typeof(long),
                oldType: "bigint");
        }
    }
}
