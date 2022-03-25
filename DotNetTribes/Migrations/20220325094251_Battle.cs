using Microsoft.EntityFrameworkCore.Migrations;

namespace DotNetTribes.Migrations
{
    public partial class Battle : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Battles",
                columns: table => new
                {
                    BattleId = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    AttackerId = table.Column<int>(type: "int", nullable: false),
                    DefenderId = table.Column<int>(type: "int", nullable: false),
                    FightStart = table.Column<long>(type: "bigint", nullable: false),
                    ArriveAt = table.Column<long>(type: "bigint", nullable: false),
                    ReturnAt = table.Column<long>(type: "bigint", nullable: false),
                    WinnerId = table.Column<int>(type: "int", nullable: false),
                    FoodStolen = table.Column<int>(type: "int", nullable: false),
                    GoldStolen = table.Column<int>(type: "int", nullable: false),
                    LostTroopsDefender = table.Column<int>(type: "int", nullable: false),
                    LostTroopsAttacker = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Battles", x => x.BattleId);
                });
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Battles");
        }
    }
}
