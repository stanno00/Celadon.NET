using Microsoft.EntityFrameworkCore.Migrations;

namespace DotNetTribes.Migrations
{
    public partial class AddRules : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "GameRules",
                columns: table => new
                {
                    GameRulesId = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Name = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    StartingGold = table.Column<int>(type: "int", nullable: false),
                    TownhallAllLevelsCost = table.Column<int>(type: "int", nullable: false),
                    FarmAllLevelsCost = table.Column<int>(type: "int", nullable: false),
                    MineAllLevelsCost = table.Column<int>(type: "int", nullable: false),
                    AcademyLevelOneCost = table.Column<int>(type: "int", nullable: false),
                    AcademyLevelNCost = table.Column<int>(type: "int", nullable: false),
                    TroopAllLevelsCost = table.Column<int>(type: "int", nullable: false),
                    TownhallLevelOneDuration = table.Column<int>(type: "int", nullable: false),
                    TownhallLevelNDuration = table.Column<int>(type: "int", nullable: false),
                    FarmAllLevelsDuration = table.Column<int>(type: "int", nullable: false),
                    MineAllLevesDuration = table.Column<int>(type: "int", nullable: false),
                    AcademyLevelOneDuration = table.Column<int>(type: "int", nullable: false),
                    AcademyLevelNDuration = table.Column<int>(type: "int", nullable: false),
                    TroopAllLevelsDuration = table.Column<int>(type: "int", nullable: false),
                    TownhallHP = table.Column<int>(type: "int", nullable: false),
                    FarmHP = table.Column<int>(type: "int", nullable: false),
                    MineHP = table.Column<int>(type: "int", nullable: false),
                    AcademyHP = table.Column<int>(type: "int", nullable: false),
                    TroopHP = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_GameRules", x => x.GameRulesId);
                });

            migrationBuilder.CreateTable(
                name: "Kingdoms",
                columns: table => new
                {
                    KingdomId = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Name = table.Column<string>(type: "nvarchar(max)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Kingdoms", x => x.KingdomId);
                });

            migrationBuilder.CreateTable(
                name: "Buildings",
                columns: table => new
                {
                    BuildingId = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Type = table.Column<int>(type: "int", nullable: false),
                    Level = table.Column<int>(type: "int", nullable: false),
                    Hp = table.Column<int>(type: "int", nullable: false),
                    Started_at = table.Column<long>(type: "bigint", nullable: false),
                    Finished_at = table.Column<long>(type: "bigint", nullable: false),
                    KingdomId = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Buildings", x => x.BuildingId);
                    table.ForeignKey(
                        name: "FK_Buildings_Kingdoms_KingdomId",
                        column: x => x.KingdomId,
                        principalTable: "Kingdoms",
                        principalColumn: "KingdomId",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "Resources",
                columns: table => new
                {
                    ResourceId = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Type = table.Column<string>(type: "nvarchar(20)", nullable: false),
                    Amount = table.Column<int>(type: "int", nullable: false),
                    Generation = table.Column<int>(type: "int", nullable: false),
                    UpdatedAt = table.Column<long>(type: "bigint", nullable: false),
                    KingdomId = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Resources", x => x.ResourceId);
                    table.ForeignKey(
                        name: "FK_Resources_Kingdoms_KingdomId",
                        column: x => x.KingdomId,
                        principalTable: "Kingdoms",
                        principalColumn: "KingdomId",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "Troops",
                columns: table => new
                {
                    TroopId = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    KingdomId = table.Column<int>(type: "int", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Troops", x => x.TroopId);
                    table.ForeignKey(
                        name: "FK_Troops_Kingdoms_KingdomId",
                        column: x => x.KingdomId,
                        principalTable: "Kingdoms",
                        principalColumn: "KingdomId",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "Users",
                columns: table => new
                {
                    UserId = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Username = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    HashedPassword = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Email = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    KingdomId = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Users", x => x.UserId);
                    table.ForeignKey(
                        name: "FK_Users_Kingdoms_KingdomId",
                        column: x => x.KingdomId,
                        principalTable: "Kingdoms",
                        principalColumn: "KingdomId",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.InsertData(
                table: "GameRules",
                columns: new[] { "GameRulesId", "AcademyHP", "AcademyLevelNCost", "AcademyLevelNDuration", "AcademyLevelOneCost", "AcademyLevelOneDuration", "FarmAllLevelsCost", "FarmAllLevelsDuration", "FarmHP", "MineAllLevelsCost", "MineAllLevesDuration", "MineHP", "Name", "StartingGold", "TownhallAllLevelsCost", "TownhallHP", "TownhallLevelNDuration", "TownhallLevelOneDuration", "TroopAllLevelsCost", "TroopAllLevelsDuration", "TroopHP" },
                values: new object[] { 1, 150, 100, 60, 150, 90, 100, 60, 100, 100, 60, 100, "Production", 500, 200, 200, 60, 120, 25, 30, 20 });

            migrationBuilder.CreateIndex(
                name: "IX_Buildings_KingdomId",
                table: "Buildings",
                column: "KingdomId");

            migrationBuilder.CreateIndex(
                name: "IX_Resources_KingdomId",
                table: "Resources",
                column: "KingdomId");

            migrationBuilder.CreateIndex(
                name: "IX_Troops_KingdomId",
                table: "Troops",
                column: "KingdomId");

            migrationBuilder.CreateIndex(
                name: "IX_Users_KingdomId",
                table: "Users",
                column: "KingdomId",
                unique: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Buildings");

            migrationBuilder.DropTable(
                name: "GameRules");

            migrationBuilder.DropTable(
                name: "Resources");

            migrationBuilder.DropTable(
                name: "Troops");

            migrationBuilder.DropTable(
                name: "Users");

            migrationBuilder.DropTable(
                name: "Kingdoms");
        }
    }
}
