using Microsoft.EntityFrameworkCore.Migrations;

namespace DotNetTribes.Migrations
{
    public partial class RenameOffer : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "SellerId",
                table: "Offers",
                newName: "SellerKingdomId");

            migrationBuilder.RenameColumn(
                name: "Expire_at",
                table: "Offers",
                newName: "ExpireAt");

            migrationBuilder.RenameColumn(
                name: "Created_at",
                table: "Offers",
                newName: "CreatedAt");

            migrationBuilder.RenameColumn(
                name: "BuyerId",
                table: "Offers",
                newName: "BuyerKingdomId");

            migrationBuilder.AddColumn<bool>(
                name: "ResourceReturned",
                table: "Offers",
                type: "bit",
                nullable: false,
                defaultValue: false);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "ResourceReturned",
                table: "Offers");

            migrationBuilder.RenameColumn(
                name: "SellerKingdomId",
                table: "Offers",
                newName: "SellerId");

            migrationBuilder.RenameColumn(
                name: "ExpireAt",
                table: "Offers",
                newName: "Expire_at");

            migrationBuilder.RenameColumn(
                name: "CreatedAt",
                table: "Offers",
                newName: "Created_at");

            migrationBuilder.RenameColumn(
                name: "BuyerKingdomId",
                table: "Offers",
                newName: "BuyerId");
        }
    }
}
