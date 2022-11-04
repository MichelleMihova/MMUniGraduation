using Microsoft.EntityFrameworkCore.Migrations;

namespace MMUniGraduation.Migrations
{
    public partial class CreateUserRole : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "LectorID",
                table: "Courses",
                type: "int",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_Courses_LectorID",
                table: "Courses",
                column: "LectorID");

            migrationBuilder.AddForeignKey(
                name: "FK_Courses_Lectors_LectorID",
                table: "Courses",
                column: "LectorID",
                principalTable: "Lectors",
                principalColumn: "ID",
                onDelete: ReferentialAction.Restrict);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Courses_Lectors_LectorID",
                table: "Courses");

            migrationBuilder.DropIndex(
                name: "IX_Courses_LectorID",
                table: "Courses");

            migrationBuilder.DropColumn(
                name: "LectorID",
                table: "Courses");
        }
    }
}
