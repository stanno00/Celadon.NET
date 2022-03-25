using Microsoft.EntityFrameworkCore.Migrations;

namespace DotNetTribes.Migrations
{
    public partial class Offer : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "MarketplaceAllLevelsCost",
                table: "GameRules",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<int>(
                name: "MarketplaceAllLevelsDuration",
                table: "GameRules",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<int>(
                name: "MarketplaceHP",
                table: "GameRules",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<int>(
                name: "MarketplaceLevelOneCost",
                table: "GameRules",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<int>(
                name: "MarketplaceLevelOneDuration",
                table: "GameRules",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<int>(
                name: "MarketplaceMaxResources",
                table: "GameRules",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.CreateTable(
                name: "Offers",
                columns: table => new
                {
                    OfferId = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    TypeOffered = table.Column<string>(type: "nvarchar(20)", nullable: false),
                    AmountOffered = table.Column<int>(type: "int", nullable: false),
                    UserOfferId = table.Column<int>(type: "int", nullable: false),
                    TypeRequired = table.Column<string>(type: "nvarchar(20)", nullable: false),
                    AmountRequired = table.Column<int>(type: "int", nullable: false),
                    UserAcceptedId = table.Column<int>(type: "int", nullable: true),
                    Started_at = table.Column<long>(type: "bigint", nullable: false),
                    Finished_at = table.Column<long>(type: "bigint", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Offers", x => x.OfferId);
                });

            migrationBuilder.UpdateData(
                table: "GameRules",
                keyColumn: "GameRulesId",
                keyValue: 1,
                columns: new[] { "MarketplaceAllLevelsCost", "MarketplaceAllLevelsDuration", "MarketplaceHP", "MarketplaceLevelOneCost", "MarketplaceLevelOneDuration", "MarketplaceMaxResources" },
                values: new object[] { 100, 10, 100, 1, 1, 75 });
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Offers");

            migrationBuilder.DropColumn(
                name: "MarketplaceAllLevelsCost",
                table: "GameRules");

            migrationBuilder.DropColumn(
                name: "MarketplaceAllLevelsDuration",
                table: "GameRules");

            migrationBuilder.DropColumn(
                name: "MarketplaceHP",
                table: "GameRules");

            migrationBuilder.DropColumn(
                name: "MarketplaceLevelOneCost",
                table: "GameRules");

            migrationBuilder.DropColumn(
                name: "MarketplaceLevelOneDuration",
                table: "GameRules");

            migrationBuilder.DropColumn(
                name: "MarketplaceMaxResources",
                table: "GameRules");
        }
    }
}
