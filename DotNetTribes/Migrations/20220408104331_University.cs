using Microsoft.EntityFrameworkCore.Migrations;

namespace DotNetTribes.Migrations
{
    public partial class University : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "AllTroopsAtkBonusCost",
                table: "GameRules",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<int>(
                name: "AllTroopsAtkBonusDuration",
                table: "GameRules",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<int>(
                name: "AllTroopsDefBonusCost",
                table: "GameRules",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<int>(
                name: "AllTroopsDefBonusDuration",
                table: "GameRules",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<int>(
                name: "BuildingBuildSpeedCost",
                table: "GameRules",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<int>(
                name: "BuildingBuildSpeedDuration",
                table: "GameRules",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<int>(
                name: "FarmProduceBonusCost",
                table: "GameRules",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<int>(
                name: "FarmProduceBonusDuration",
                table: "GameRules",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<int>(
                name: "MineProduceBonusCost",
                table: "GameRules",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<int>(
                name: "MineProduceBonusDuration",
                table: "GameRules",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<int>(
                name: "TroopsTrainSpeedCost",
                table: "GameRules",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<int>(
                name: "TroopsTrainSpeedDuration",
                table: "GameRules",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<int>(
                name: "UniversityAllLevelCost",
                table: "GameRules",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<int>(
                name: "UniversityAllLevelDuration",
                table: "GameRules",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<int>(
                name: "UniversityHP",
                table: "GameRules",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.CreateTable(
                name: "UniversityUpgrades",
                columns: table => new
                {
                    UniversityUpgradeId = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    UpgradeType = table.Column<string>(type: "nvarchar(20)", nullable: false),
                    AffectStrength = table.Column<double>(type: "float", nullable: false),
                    Level = table.Column<int>(type: "int", nullable: false),
                    KingdomId = table.Column<int>(type: "int", nullable: false),
                    StartedAt = table.Column<long>(type: "bigint", nullable: false),
                    FinishedAt = table.Column<long>(type: "bigint", nullable: false),
                    AddedToKingdom = table.Column<bool>(type: "bit", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_UniversityUpgrades", x => x.UniversityUpgradeId);
                    table.ForeignKey(
                        name: "FK_UniversityUpgrades_Kingdoms_KingdomId",
                        column: x => x.KingdomId,
                        principalTable: "Kingdoms",
                        principalColumn: "KingdomId",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.UpdateData(
                table: "GameRules",
                keyColumn: "GameRulesId",
                keyValue: 1,
                columns: new[] { "AllTroopsAtkBonusCost", "AllTroopsAtkBonusDuration", "AllTroopsDefBonusCost", "AllTroopsDefBonusDuration", "BuildingBuildSpeedCost", "BuildingBuildSpeedDuration", "FarmProduceBonusCost", "FarmProduceBonusDuration", "MineProduceBonusCost", "MineProduceBonusDuration", "TroopsTrainSpeedCost", "TroopsTrainSpeedDuration", "UniversityAllLevelCost", "UniversityAllLevelDuration", "UniversityHP" },
                values: new object[] { 10, 10, 10, 10, 10, 10, 10, 10, 10, 10, 10, 10, 300, 10, 200 });

            migrationBuilder.CreateIndex(
                name: "IX_UniversityUpgrades_KingdomId",
                table: "UniversityUpgrades",
                column: "KingdomId");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "UniversityUpgrades");

            migrationBuilder.DropColumn(
                name: "AllTroopsAtkBonusCost",
                table: "GameRules");

            migrationBuilder.DropColumn(
                name: "AllTroopsAtkBonusDuration",
                table: "GameRules");

            migrationBuilder.DropColumn(
                name: "AllTroopsDefBonusCost",
                table: "GameRules");

            migrationBuilder.DropColumn(
                name: "AllTroopsDefBonusDuration",
                table: "GameRules");

            migrationBuilder.DropColumn(
                name: "BuildingBuildSpeedCost",
                table: "GameRules");

            migrationBuilder.DropColumn(
                name: "BuildingBuildSpeedDuration",
                table: "GameRules");

            migrationBuilder.DropColumn(
                name: "FarmProduceBonusCost",
                table: "GameRules");

            migrationBuilder.DropColumn(
                name: "FarmProduceBonusDuration",
                table: "GameRules");

            migrationBuilder.DropColumn(
                name: "MineProduceBonusCost",
                table: "GameRules");

            migrationBuilder.DropColumn(
                name: "MineProduceBonusDuration",
                table: "GameRules");

            migrationBuilder.DropColumn(
                name: "TroopsTrainSpeedCost",
                table: "GameRules");

            migrationBuilder.DropColumn(
                name: "TroopsTrainSpeedDuration",
                table: "GameRules");

            migrationBuilder.DropColumn(
                name: "UniversityAllLevelCost",
                table: "GameRules");

            migrationBuilder.DropColumn(
                name: "UniversityAllLevelDuration",
                table: "GameRules");

            migrationBuilder.DropColumn(
                name: "UniversityHP",
                table: "GameRules");
        }
    }
}
