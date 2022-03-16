using Microsoft.EntityFrameworkCore.Migrations;

namespace DotNetTribes.Migrations
{
    public partial class Changedresourceprops : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "CreatedAt",
                table: "Resources");

            migrationBuilder.AddColumn<long>(
                name: "UpdatedAt",
                table: "Resources",
                type: "bigint",
                nullable: false,
                defaultValue: 0L);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "UpdatedAt",
                table: "Resources");

            migrationBuilder.AddColumn<int>(
                name: "CreatedAt",
                table: "Resources",
                type: "int",
                nullable: false,
                defaultValue: 0);
        }
    }
}
