using Microsoft.EntityFrameworkCore.Migrations;

namespace MMUniGraduation.Migrations
{
    public partial class RemoveLectorStudyProgram : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Courses_Students_StudentId",
                table: "Courses");

            migrationBuilder.DropTable(
                name: "LectorStudyProgram");

            migrationBuilder.DropIndex(
                name: "IX_Courses_StudentId",
                table: "Courses");

            migrationBuilder.DropColumn(
                name: "StudentId",
                table: "Courses");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "StudentId",
                table: "Courses",
                type: "int",
                nullable: true);

            migrationBuilder.CreateTable(
                name: "LectorStudyProgram",
                columns: table => new
                {
                    LectorId = table.Column<int>(type: "int", nullable: false),
                    StudyProgramId = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_LectorStudyProgram", x => new { x.LectorId, x.StudyProgramId });
                    table.ForeignKey(
                        name: "FK_LectorStudyProgram_Lectors_LectorId",
                        column: x => x.LectorId,
                        principalTable: "Lectors",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_LectorStudyProgram_StudyPrograms_StudyProgramId",
                        column: x => x.StudyProgramId,
                        principalTable: "StudyPrograms",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_Courses_StudentId",
                table: "Courses",
                column: "StudentId");

            migrationBuilder.CreateIndex(
                name: "IX_LectorStudyProgram_StudyProgramId",
                table: "LectorStudyProgram",
                column: "StudyProgramId");

            migrationBuilder.AddForeignKey(
                name: "FK_Courses_Students_StudentId",
                table: "Courses",
                column: "StudentId",
                principalTable: "Students",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }
    }
}
