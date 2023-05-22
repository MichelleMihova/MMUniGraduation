using Microsoft.EntityFrameworkCore.Migrations;

namespace MMUniGraduation.Migrations
{
    public partial class RemoveCurrentCourseIdFromStudent : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Students_Courses_CurrentCourseId",
                table: "Students");

            migrationBuilder.DropIndex(
                name: "IX_Students_CurrentCourseId",
                table: "Students");

            migrationBuilder.DropColumn(
                name: "CurrentCourseId",
                table: "Students");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "CurrentCourseId",
                table: "Students",
                type: "int",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_Students_CurrentCourseId",
                table: "Students",
                column: "CurrentCourseId");

            migrationBuilder.AddForeignKey(
                name: "FK_Students_Courses_CurrentCourseId",
                table: "Students",
                column: "CurrentCourseId",
                principalTable: "Courses",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }
    }
}
