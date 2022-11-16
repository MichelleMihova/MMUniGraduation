using Microsoft.EntityFrameworkCore.Migrations;

namespace MMUniGraduation.Migrations
{
    public partial class changesInLectureFile : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<decimal>(
                name: "MinHWGrade",
                table: "LectureFiles",
                type: "decimal(18,2)",
                nullable: false,
                defaultValue: 0m);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "MinHWGrade",
                table: "LectureFiles");
        }
    }
}
