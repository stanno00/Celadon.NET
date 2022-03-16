using Microsoft.EntityFrameworkCore.Migrations;

namespace DotNetTribes.Migrations
{
    public partial class ChangeResourceType : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Buildings_Kingdoms_KingdomId",
                table: "Buildings");

            migrationBuilder.AlterColumn<string>(
                name: "Type",
                table: "Resources",
                type: "nvarchar(20)",
                nullable: false,
                defaultValue: "",
                oldClrType: typeof(string),
                oldType: "nvarchar(max)",
                oldNullable: true);

            migrationBuilder.AlterColumn<int>(
                name: "KingdomId",
                table: "Buildings",
                type: "int",
                nullable: false,
                defaultValue: 0,
                oldClrType: typeof(int),
                oldType: "int",
                oldNullable: true);

            migrationBuilder.AlterColumn<int>(
                name: "BuildingId",
                table: "Buildings",
                type: "int",
                nullable: false,
                oldClrType: typeof(long),
                oldType: "bigint")
                .Annotation("SqlServer:Identity", "1, 1")
                .OldAnnotation("SqlServer:Identity", "1, 1");

            migrationBuilder.AddColumn<long>(
                name: "Finished_at",
                table: "Buildings",
                type: "bigint",
                nullable: false,
                defaultValue: 0L);

            migrationBuilder.AddColumn<int>(
                name: "Hp",
                table: "Buildings",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<int>(
                name: "Level",
                table: "Buildings",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<long>(
                name: "Started_at",
                table: "Buildings",
                type: "bigint",
                nullable: false,
                defaultValue: 0L);

            migrationBuilder.AddColumn<int>(
                name: "Type",
                table: "Buildings",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddForeignKey(
                name: "FK_Buildings_Kingdoms_KingdomId",
                table: "Buildings",
                column: "KingdomId",
                principalTable: "Kingdoms",
                principalColumn: "KingdomId",
                onDelete: ReferentialAction.Cascade);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Buildings_Kingdoms_KingdomId",
                table: "Buildings");

            migrationBuilder.DropColumn(
                name: "Finished_at",
                table: "Buildings");

            migrationBuilder.DropColumn(
                name: "Hp",
                table: "Buildings");

            migrationBuilder.DropColumn(
                name: "Level",
                table: "Buildings");

            migrationBuilder.DropColumn(
                name: "Started_at",
                table: "Buildings");

            migrationBuilder.DropColumn(
                name: "Type",
                table: "Buildings");

            migrationBuilder.AlterColumn<string>(
                name: "Type",
                table: "Resources",
                type: "nvarchar(max)",
                nullable: true,
                oldClrType: typeof(string),
                oldType: "nvarchar(20)");

            migrationBuilder.AlterColumn<int>(
                name: "KingdomId",
                table: "Buildings",
                type: "int",
                nullable: true,
                oldClrType: typeof(int),
                oldType: "int");

            migrationBuilder.AlterColumn<long>(
                name: "BuildingId",
                table: "Buildings",
                type: "bigint",
                nullable: false,
                oldClrType: typeof(int),
                oldType: "int")
                .Annotation("SqlServer:Identity", "1, 1")
                .OldAnnotation("SqlServer:Identity", "1, 1");

            migrationBuilder.AddForeignKey(
                name: "FK_Buildings_Kingdoms_KingdomId",
                table: "Buildings",
                column: "KingdomId",
                principalTable: "Kingdoms",
                principalColumn: "KingdomId",
                onDelete: ReferentialAction.Restrict);
        }
    }
}
