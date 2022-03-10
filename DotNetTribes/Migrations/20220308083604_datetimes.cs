using Microsoft.EntityFrameworkCore.Migrations;

namespace DotNetTribes.Migrations
{
    public partial class datetimes : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "UpdatedAt",
                table: "Resources");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "UpdatedAt",
                table: "Resources",
                type: "int",
                nullable: false,
                defaultValue: 0);
        }
    }
}
