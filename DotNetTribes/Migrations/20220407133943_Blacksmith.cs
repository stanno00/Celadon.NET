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
                name: "CostTroopRanger",
                table: "GameRules",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<int>(
                name: "CostTroopScout",
                table: "GameRules",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<int>(
                name: "TimeUpgradeTroopRanger",
                table: "GameRules",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<int>(
                name: "TimeUpgradeTroopScout",
                table: "GameRules",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<int>(
                name: "TrainingTimeTroopRanger",
                table: "GameRules",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<int>(
                name: "TrainingTimeTroopScout",
                table: "GameRules",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<int>(
                name: "TroopRangerHp",
                table: "GameRules",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<int>(
                name: "TroopScoutHp",
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

            migrationBuilder.AddColumn<int>(
                name: "UpgradeForTroopRanger",
                table: "GameRules",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.CreateTable(
                name: "BuildingUpgrade",
                columns: table => new
                {
                    BuildingUpgradeId = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Name = table.Column<int>(type: "int", nullable: false),
                    StartedAt = table.Column<long>(type: "bigint", nullable: false),
                    FinishedAt = table.Column<long>(type: "bigint", nullable: false),
                    KingdomId = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_BuildingUpgrade", x => x.BuildingUpgradeId);
                    table.ForeignKey(
                        name: "FK_BuildingUpgrade_Kingdoms_KingdomId",
                        column: x => x.KingdomId,
                        principalTable: "Kingdoms",
                        principalColumn: "KingdomId",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.UpdateData(
                table: "GameRules",
                keyColumn: "GameRulesId",
                keyValue: 1,
                columns: new[] { "BlacksmithHp", "BlacksmithLevelOneCost", "BlacksmithLevelOneDuration", "CostTroopRanger", "CostTroopScout", "TimeUpgradeTroopRanger", "TimeUpgradeTroopScout", "TrainingTimeTroopRanger", "TrainingTimeTroopScout", "TroopRangerHp", "TroopScoutHp", "UpgradeForSpecialTroopScout", "UpgradeForTroopRanger" },
                values: new object[] { 150, 500, 600, 50, 100, 43200, 72000, 200, 300, 10, 1, 1000, 2000 });

            migrationBuilder.CreateIndex(
                name: "IX_BuildingUpgrade_KingdomId",
                table: "BuildingUpgrade",
                column: "KingdomId");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "BuildingUpgrade");

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
                name: "CostTroopRanger",
                table: "GameRules");

            migrationBuilder.DropColumn(
                name: "CostTroopScout",
                table: "GameRules");

            migrationBuilder.DropColumn(
                name: "TimeUpgradeTroopRanger",
                table: "GameRules");

            migrationBuilder.DropColumn(
                name: "TimeUpgradeTroopScout",
                table: "GameRules");

            migrationBuilder.DropColumn(
                name: "TrainingTimeTroopRanger",
                table: "GameRules");

            migrationBuilder.DropColumn(
                name: "TrainingTimeTroopScout",
                table: "GameRules");

            migrationBuilder.DropColumn(
                name: "TroopRangerHp",
                table: "GameRules");

            migrationBuilder.DropColumn(
                name: "TroopScoutHp",
                table: "GameRules");

            migrationBuilder.DropColumn(
                name: "UpgradeForSpecialTroopScout",
                table: "GameRules");

            migrationBuilder.DropColumn(
                name: "UpgradeForTroopRanger",
                table: "GameRules");
        }
    }
}
