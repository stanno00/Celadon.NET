using Microsoft.EntityFrameworkCore.Migrations;

namespace VaccineTask.Migrations
{
    public partial class VaccineOrderChanges : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "HospitalName",
                table: "HospitalVaccine");

            migrationBuilder.DropColumn(
                name: "VaccineName",
                table: "HospitalVaccine");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "HospitalName",
                table: "HospitalVaccine",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "VaccineName",
                table: "HospitalVaccine",
                type: "nvarchar(max)",
                nullable: true);
        }
    }
}
