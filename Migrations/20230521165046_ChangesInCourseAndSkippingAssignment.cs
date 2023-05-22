using Microsoft.EntityFrameworkCore.Migrations;

namespace MMUniGraduation.Migrations
{
    public partial class ChangesInCourseAndSkippingAssignment : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Courses_Lectors_LectorId",
                table: "Courses");

            migrationBuilder.DropTable(
                name: "StudentStudyProgram");

            migrationBuilder.DropIndex(
                name: "IX_Courses_LectorId",
                table: "Courses");

            migrationBuilder.DropColumn(
                name: "LectorId",
                table: "Courses");

            migrationBuilder.AddColumn<int>(
                name: "CourseId",
                table: "LectureFiles",
                type: "int",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_LectureFiles_CourseId",
                table: "LectureFiles",
                column: "CourseId");

            migrationBuilder.AddForeignKey(
                name: "FK_LectureFiles_Courses_CourseId",
                table: "LectureFiles",
                column: "CourseId",
                principalTable: "Courses",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_LectureFiles_Courses_CourseId",
                table: "LectureFiles");

            migrationBuilder.DropIndex(
                name: "IX_LectureFiles_CourseId",
                table: "LectureFiles");

            migrationBuilder.DropColumn(
                name: "CourseId",
                table: "LectureFiles");

            migrationBuilder.AddColumn<int>(
                name: "LectorId",
                table: "Courses",
                type: "int",
                nullable: true);

            migrationBuilder.CreateTable(
                name: "StudentStudyProgram",
                columns: table => new
                {
                    StudentId = table.Column<int>(type: "int", nullable: false),
                    StudyProgramId = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_StudentStudyProgram", x => new { x.StudentId, x.StudyProgramId });
                    table.ForeignKey(
                        name: "FK_StudentStudyProgram_Students_StudentId",
                        column: x => x.StudentId,
                        principalTable: "Students",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_StudentStudyProgram_StudyPrograms_StudyProgramId",
                        column: x => x.StudyProgramId,
                        principalTable: "StudyPrograms",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_Courses_LectorId",
                table: "Courses",
                column: "LectorId");

            migrationBuilder.CreateIndex(
                name: "IX_StudentStudyProgram_StudyProgramId",
                table: "StudentStudyProgram",
                column: "StudyProgramId");

            migrationBuilder.AddForeignKey(
                name: "FK_Courses_Lectors_LectorId",
                table: "Courses",
                column: "LectorId",
                principalTable: "Lectors",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }
    }
}
