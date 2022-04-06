using Microsoft.EntityFrameworkCore.Migrations;

namespace DotNetTribes.Migrations
{
    public partial class Blacksmith : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "Name",
                table: "Troops",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "BlacksmithHp",
                table: "GameRules",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<int>(
                name: "BlacksmithLevelOneCost",
                table: "GameRules",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<int>(
                name: "BlacksmithLevelOneDuration",
                table: "GameRules",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<int>(
                name: "CostSpecialTroopRanger",
                table: "GameRules",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<int>(
                name: "CostSpecialTroopScout",
                table: "GameRules",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<int>(
                name: "SpecialTroopRangerHp",
                table: "GameRules",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<int>(
                name: "SpecialTroopScoutHp",
                table: "GameRules",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<int>(
                name: "TimeUpgradeForSpecialTroopRanger",
                table: "GameRules",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<int>(
                name: "TimeUpgradeForSpecialTroopScout",
                table: "GameRules",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<int>(
                name: "TrainingTimeSpecialTroopRanger",
                table: "GameRules",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<int>(
                name: "TrainingTimeSpecialTroopScout",
                table: "GameRules",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<int>(
                name: "UpgradeForSpecialTroopRanger",
                table: "GameRules",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<int>(
                name: "UpgradeForSpecialTroopScout",
                table: "GameRules",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.CreateTable(
                name: "BuildingUpgrades",
                columns: table => new
                {
                    BuildingUpgradesId = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Name = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    StartedAt = table.Column<long>(type: "bigint", nullable: false),
                    FinishedAt = table.Column<long>(type: "bigint", nullable: false),
                    KingdomId = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_BuildingUpgrades", x => x.BuildingUpgradesId);
                    table.ForeignKey(
                        name: "FK_BuildingUpgrades_Kingdoms_KingdomId",
                        column: x => x.KingdomId,
                        principalTable: "Kingdoms",
                        principalColumn: "KingdomId",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.UpdateData(
                table: "GameRules",
                keyColumn: "GameRulesId",
                keyValue: 1,
                columns: new[] { "BlacksmithHp", "BlacksmithLevelOneCost", "BlacksmithLevelOneDuration", "CostSpecialTroopRanger", "CostSpecialTroopScout", "SpecialTroopRangerHp", "SpecialTroopScoutHp", "TimeUpgradeForSpecialTroopRanger", "TimeUpgradeForSpecialTroopScout", "TrainingTimeSpecialTroopRanger", "TrainingTimeSpecialTroopScout", "UpgradeForSpecialTroopRanger", "UpgradeForSpecialTroopScout" },
                values: new object[] { 150, 500, 600, 50, 100, 10, 1, 43200, 72000, 200, 300, 2000, 1000 });

            migrationBuilder.CreateIndex(
                name: "IX_BuildingUpgrades_KingdomId",
                table: "BuildingUpgrades",
                column: "KingdomId");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "BuildingUpgrades");

            migrationBuilder.DropColumn(
                name: "Name",
                table: "Troops");

            migrationBuilder.DropColumn(
                name: "BlacksmithHp",
                table: "GameRules");

            migrationBuilder.DropColumn(
                name: "BlacksmithLevelOneCost",
                table: "GameRules");

            migrationBuilder.DropColumn(
                name: "BlacksmithLevelOneDuration",
                table: "GameRules");

            migrationBuilder.DropColumn(
                name: "CostSpecialTroopRanger",
                table: "GameRules");

            migrationBuilder.DropColumn(
                name: "CostSpecialTroopScout",
                table: "GameRules");

            migrationBuilder.DropColumn(
                name: "SpecialTroopRangerHp",
                table: "GameRules");

            migrationBuilder.DropColumn(
                name: "SpecialTroopScoutHp",
                table: "GameRules");

            migrationBuilder.DropColumn(
                name: "TimeUpgradeForSpecialTroopRanger",
                table: "GameRules");

            migrationBuilder.DropColumn(
                name: "TimeUpgradeForSpecialTroopScout",
                table: "GameRules");

            migrationBuilder.DropColumn(
                name: "TrainingTimeSpecialTroopRanger",
                table: "GameRules");

            migrationBuilder.DropColumn(
                name: "TrainingTimeSpecialTroopScout",
                table: "GameRules");

            migrationBuilder.DropColumn(
                name: "UpgradeForSpecialTroopRanger",
                table: "GameRules");

            migrationBuilder.DropColumn(
                name: "UpgradeForSpecialTroopScout",
                table: "GameRules");
        }
    }
}
