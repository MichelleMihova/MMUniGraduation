using Microsoft.EntityFrameworkCore.Migrations;

namespace MMUniGraduation.Migrations
{
    public partial class addIsFinal : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Exam",
                table: "Courses");

            migrationBuilder.DropColumn(
                name: "ExamGrade",
                table: "Courses");

            migrationBuilder.DropColumn(
                name: "FinalHomeworkGrade",
                table: "Courses");

            migrationBuilder.DropColumn(
                name: "UserId",
                table: "Courses");

            migrationBuilder.AddColumn<bool>(
                name: "IsFinal",
                table: "Lectures",
                type: "bit",
                nullable: false,
                defaultValue: false);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "IsFinal",
                table: "Lectures");

            migrationBuilder.AddColumn<string>(
                name: "Exam",
                table: "Courses",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<decimal>(
                name: "ExamGrade",
                table: "Courses",
                type: "decimal(18,2)",
                nullable: false,
                defaultValue: 0m);

            migrationBuilder.AddColumn<decimal>(
                name: "FinalHomeworkGrade",
                table: "Courses",
                type: "decimal(18,2)",
                nullable: false,
                defaultValue: 0m);

            migrationBuilder.AddColumn<string>(
                name: "UserId",
                table: "Courses",
                type: "nvarchar(max)",
                nullable: true);
        }
    }
}
