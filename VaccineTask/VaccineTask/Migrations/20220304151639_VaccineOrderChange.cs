using Microsoft.EntityFrameworkCore.Migrations;

namespace VaccineTask.Migrations
{
    public partial class VaccineOrderChange : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_VaccineOrders_Hospitals_HospitalId",
                table: "VaccineOrders");

            migrationBuilder.DropColumn(
                name: "VaccineId",
                table: "VaccineOrders");

            migrationBuilder.AlterColumn<int>(
                name: "HospitalId",
                table: "VaccineOrders",
                nullable: true,
                oldClrType: typeof(int),
                oldType: "int");

            migrationBuilder.AddForeignKey(
                name: "FK_VaccineOrders_Hospitals_HospitalId",
                table: "VaccineOrders",
                column: "HospitalId",
                principalTable: "Hospitals",
                principalColumn: "HospitalId",
                onDelete: ReferentialAction.Restrict);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_VaccineOrders_Hospitals_HospitalId",
                table: "VaccineOrders");

            migrationBuilder.AlterColumn<int>(
                name: "HospitalId",
                table: "VaccineOrders",
                type: "int",
                nullable: false,
                oldClrType: typeof(int),
                oldNullable: true);

            migrationBuilder.AddColumn<int>(
                name: "VaccineId",
                table: "VaccineOrders",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddForeignKey(
                name: "FK_VaccineOrders_Hospitals_HospitalId",
                table: "VaccineOrders",
                column: "HospitalId",
                principalTable: "Hospitals",
                principalColumn: "HospitalId",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
