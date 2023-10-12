using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Dental.Migrations
{
    /// <inheritdoc />
    public partial class Nullable : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_DentalScans_Patients_PatientId",
                table: "DentalScans");

            migrationBuilder.AlterColumn<int>(
                name: "PatientId",
                table: "DentalScans",
                type: "int",
                nullable: false,
                defaultValue: 0,
                oldClrType: typeof(int),
                oldType: "int",
                oldNullable: true);

            migrationBuilder.AddColumn<int>(
                name: "UserCategory",
                table: "AspNetUsers",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddForeignKey(
                name: "FK_DentalScans_Patients_PatientId",
                table: "DentalScans",
                column: "PatientId",
                principalTable: "Patients",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_DentalScans_Patients_PatientId",
                table: "DentalScans");

            migrationBuilder.DropColumn(
                name: "UserCategory",
                table: "AspNetUsers");

            migrationBuilder.AlterColumn<int>(
                name: "PatientId",
                table: "DentalScans",
                type: "int",
                nullable: true,
                oldClrType: typeof(int),
                oldType: "int");

            migrationBuilder.AddForeignKey(
                name: "FK_DentalScans_Patients_PatientId",
                table: "DentalScans",
                column: "PatientId",
                principalTable: "Patients",
                principalColumn: "Id");
        }
    }
}
