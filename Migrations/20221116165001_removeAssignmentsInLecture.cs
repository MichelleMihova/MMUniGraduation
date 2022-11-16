using Microsoft.EntityFrameworkCore.Migrations;

namespace MMUniGraduation.Migrations
{
    public partial class removeAssignmentsInLecture : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "AssignmentGrade",
                table: "Lectures");

            migrationBuilder.DropColumn(
                name: "Assignments",
                table: "Lectures");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<decimal>(
                name: "AssignmentGrade",
                table: "Lectures",
                type: "decimal(18,2)",
                nullable: false,
                defaultValue: 0m);

            migrationBuilder.AddColumn<string>(
                name: "Assignments",
                table: "Lectures",
                type: "nvarchar(max)",
                nullable: true);
        }
    }
}
