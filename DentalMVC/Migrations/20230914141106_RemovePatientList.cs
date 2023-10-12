using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Dental.Migrations
{
    /// <inheritdoc />
    public partial class RemovePatientList : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Patients_AspNetUsers_UserModelId",
                table: "Patients");

            migrationBuilder.DropIndex(
                name: "IX_Patients_UserModelId",
                table: "Patients");

            migrationBuilder.DropColumn(
                name: "UserModelId",
                table: "Patients");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "UserModelId",
                table: "Patients",
                type: "nvarchar(450)",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_Patients_UserModelId",
                table: "Patients",
                column: "UserModelId");

            migrationBuilder.AddForeignKey(
                name: "FK_Patients_AspNetUsers_UserModelId",
                table: "Patients",
                column: "UserModelId",
                principalTable: "AspNetUsers",
                principalColumn: "Id");
        }
    }
}
