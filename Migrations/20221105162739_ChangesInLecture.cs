using Microsoft.EntityFrameworkCore.Migrations;

namespace MMUniGraduation.Migrations
{
    public partial class ChangesInLecture : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_LearningObjects_Lectures_LectureId",
                table: "LearningObjects");

            migrationBuilder.DropIndex(
                name: "IX_LearningObjects_LectureId",
                table: "LearningObjects");

            migrationBuilder.DropColumn(
                name: "LectureId",
                table: "LearningObjects");

            migrationBuilder.AddColumn<string>(
                name: "VideoUrl",
                table: "Lectures",
                type: "nvarchar(max)",
                nullable: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "VideoUrl",
                table: "Lectures");

            migrationBuilder.AddColumn<int>(
                name: "LectureId",
                table: "LearningObjects",
                type: "int",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_LearningObjects_LectureId",
                table: "LearningObjects",
                column: "LectureId");

            migrationBuilder.AddForeignKey(
                name: "FK_LearningObjects_Lectures_LectureId",
                table: "LearningObjects",
                column: "LectureId",
                principalTable: "Lectures",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }
    }
}
