using Microsoft.EntityFrameworkCore.Migrations;

namespace DotNetTribes.Migrations
{
    public partial class UnfinishedTroops : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "FinishedAt",
                table: "Troops");

            migrationBuilder.DropColumn(
                name: "StartedAt",
                table: "Troops");

            migrationBuilder.AddColumn<int>(
                name: "Level",
                table: "Troops",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.CreateTable(
                name: "TroopsWorkedOn",
                columns: table => new
                {
                    UnfinishedTroopId = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    KingdomId = table.Column<int>(type: "int", nullable: false),
                    StartedAt = table.Column<long>(type: "bigint", nullable: false),
                    FinishedAt = table.Column<long>(type: "bigint", nullable: false),
                    UpdatedAt = table.Column<long>(type: "bigint", nullable: false),
                    Level = table.Column<int>(type: "int", nullable: false),
                    Upgrading = table.Column<bool>(type: "bit", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_TroopsWorkedOn", x => x.UnfinishedTroopId);
                    table.ForeignKey(
                        name: "FK_TroopsWorkedOn_Kingdoms_KingdomId",
                        column: x => x.KingdomId,
                        principalTable: "Kingdoms",
                        principalColumn: "KingdomId",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_TroopsWorkedOn_KingdomId",
                table: "TroopsWorkedOn",
                column: "KingdomId");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "TroopsWorkedOn");

            migrationBuilder.DropColumn(
                name: "Level",
                table: "Troops");

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
        }
    }
}
