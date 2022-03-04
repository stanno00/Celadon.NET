using Microsoft.EntityFrameworkCore.Migrations;

namespace VaccineTask.Migrations
{
    public partial class HospitalVaccines : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateIndex(
                name: "IX_HospitalVaccine_VaccineId",
                table: "HospitalVaccine",
                column: "VaccineId");

            migrationBuilder.AddForeignKey(
                name: "FK_HospitalVaccine_Hospitals_HospitalId",
                table: "HospitalVaccine",
                column: "HospitalId",
                principalTable: "Hospitals",
                principalColumn: "HospitalId",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_HospitalVaccine_Vaccines_VaccineId",
                table: "HospitalVaccine",
                column: "VaccineId",
                principalTable: "Vaccines",
                principalColumn: "VaccineId",
                onDelete: ReferentialAction.Cascade);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_HospitalVaccine_Hospitals_HospitalId",
                table: "HospitalVaccine");

            migrationBuilder.DropForeignKey(
                name: "FK_HospitalVaccine_Vaccines_VaccineId",
                table: "HospitalVaccine");

            migrationBuilder.DropIndex(
                name: "IX_HospitalVaccine_VaccineId",
                table: "HospitalVaccine");
        }
    }
}
