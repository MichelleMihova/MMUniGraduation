using Microsoft.EntityFrameworkCore.Migrations;

namespace MMUniGraduation.Migrations
{
    public partial class changedDB : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Courses_StudyPrograms_StudyProgramId",
                table: "Courses");

            migrationBuilder.DropForeignKey(
                name: "FK_CreateCourse_StudyPrograms_StudyProgramId",
                table: "CreateCourse");

            migrationBuilder.DropForeignKey(
                name: "FK_LearningObjects_Lectures_LectureId",
                table: "LearningObjects");

            migrationBuilder.DropForeignKey(
                name: "FK_Lectures_Courses_CourseId",
                table: "Lectures");

            migrationBuilder.DropForeignKey(
                name: "FK_RestrictAccess_Courses_CourseId",
                table: "RestrictAccess");

            migrationBuilder.DropForeignKey(
                name: "FK_Students_Courses_CurrentCourseId",
                table: "Students");

            migrationBuilder.DropPrimaryKey(
                name: "PK_Lectures",
                table: "Lectures");

            migrationBuilder.DropIndex(
                name: "IX_Lectures_CourseId",
                table: "Lectures");

            migrationBuilder.DropIndex(
                name: "IX_CreateCourse_StudyProgramId",
                table: "CreateCourse");

            migrationBuilder.DropPrimaryKey(
                name: "PK_Courses",
                table: "Courses");

            migrationBuilder.DropIndex(
                name: "IX_Courses_StudyProgramId",
                table: "Courses");

            migrationBuilder.RenameColumn(
                name: "CurrentCourseId",
                table: "Students",
                newName: "CurrentCourseStudyProgramId");

            migrationBuilder.RenameIndex(
                name: "IX_Students_CurrentCourseId",
                table: "Students",
                newName: "IX_Students_CurrentCourseStudyProgramId");

            migrationBuilder.RenameColumn(
                name: "CourseId",
                table: "RestrictAccess",
                newName: "CourseStudyProgramId");

            migrationBuilder.RenameIndex(
                name: "IX_RestrictAccess_CourseId",
                table: "RestrictAccess",
                newName: "IX_RestrictAccess_CourseStudyProgramId");

            migrationBuilder.RenameColumn(
                name: "LectureId",
                table: "LearningObjects",
                newName: "LectureCourseId");

            migrationBuilder.RenameIndex(
                name: "IX_LearningObjects_LectureId",
                table: "LearningObjects",
                newName: "IX_LearningObjects_LectureCourseId");

            migrationBuilder.AlterColumn<int>(
                name: "CourseId",
                table: "Lectures",
                type: "int",
                nullable: false,
                defaultValue: 0,
                oldClrType: typeof(int),
                oldType: "int",
                oldNullable: true);

            migrationBuilder.AlterColumn<int>(
                name: "Id",
                table: "Lectures",
                type: "int",
                nullable: false,
                oldClrType: typeof(int),
                oldType: "int")
                .OldAnnotation("SqlServer:Identity", "1, 1");

            migrationBuilder.AlterColumn<int>(
                name: "StudyProgramId",
                table: "CreateCourse",
                type: "int",
                nullable: false,
                defaultValue: 0,
                oldClrType: typeof(int),
                oldType: "int",
                oldNullable: true);

            migrationBuilder.AlterColumn<int>(
                name: "StudyProgramId",
                table: "Courses",
                type: "int",
                nullable: false,
                defaultValue: 0,
                oldClrType: typeof(int),
                oldType: "int",
                oldNullable: true);

            migrationBuilder.AlterColumn<int>(
                name: "Id",
                table: "Courses",
                type: "int",
                nullable: false,
                oldClrType: typeof(int),
                oldType: "int")
                .OldAnnotation("SqlServer:Identity", "1, 1");

            migrationBuilder.AddPrimaryKey(
                name: "PK_Lectures",
                table: "Lectures",
                column: "CourseId");

            migrationBuilder.AddPrimaryKey(
                name: "PK_Courses",
                table: "Courses",
                column: "StudyProgramId");

            migrationBuilder.AddForeignKey(
                name: "FK_Courses_StudyPrograms_StudyProgramId",
                table: "Courses",
                column: "StudyProgramId",
                principalTable: "StudyPrograms",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_LearningObjects_Lectures_LectureCourseId",
                table: "LearningObjects",
                column: "LectureCourseId",
                principalTable: "Lectures",
                principalColumn: "CourseId",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_Lectures_Courses_CourseId",
                table: "Lectures",
                column: "CourseId",
                principalTable: "Courses",
                principalColumn: "StudyProgramId",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_RestrictAccess_Courses_CourseStudyProgramId",
                table: "RestrictAccess",
                column: "CourseStudyProgramId",
                principalTable: "Courses",
                principalColumn: "StudyProgramId",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_Students_Courses_CurrentCourseStudyProgramId",
                table: "Students",
                column: "CurrentCourseStudyProgramId",
                principalTable: "Courses",
                principalColumn: "StudyProgramId",
                onDelete: ReferentialAction.Restrict);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Courses_StudyPrograms_StudyProgramId",
                table: "Courses");

            migrationBuilder.DropForeignKey(
                name: "FK_LearningObjects_Lectures_LectureCourseId",
                table: "LearningObjects");

            migrationBuilder.DropForeignKey(
                name: "FK_Lectures_Courses_CourseId",
                table: "Lectures");

            migrationBuilder.DropForeignKey(
                name: "FK_RestrictAccess_Courses_CourseStudyProgramId",
                table: "RestrictAccess");

            migrationBuilder.DropForeignKey(
                name: "FK_Students_Courses_CurrentCourseStudyProgramId",
                table: "Students");

            migrationBuilder.DropPrimaryKey(
                name: "PK_Lectures",
                table: "Lectures");

            migrationBuilder.DropPrimaryKey(
                name: "PK_Courses",
                table: "Courses");

            migrationBuilder.RenameColumn(
                name: "CurrentCourseStudyProgramId",
                table: "Students",
                newName: "CurrentCourseId");

            migrationBuilder.RenameIndex(
                name: "IX_Students_CurrentCourseStudyProgramId",
                table: "Students",
                newName: "IX_Students_CurrentCourseId");

            migrationBuilder.RenameColumn(
                name: "CourseStudyProgramId",
                table: "RestrictAccess",
                newName: "CourseId");

            migrationBuilder.RenameIndex(
                name: "IX_RestrictAccess_CourseStudyProgramId",
                table: "RestrictAccess",
                newName: "IX_RestrictAccess_CourseId");

            migrationBuilder.RenameColumn(
                name: "LectureCourseId",
                table: "LearningObjects",
                newName: "LectureId");

            migrationBuilder.RenameIndex(
                name: "IX_LearningObjects_LectureCourseId",
                table: "LearningObjects",
                newName: "IX_LearningObjects_LectureId");

            migrationBuilder.AlterColumn<int>(
                name: "Id",
                table: "Lectures",
                type: "int",
                nullable: false,
                oldClrType: typeof(int),
                oldType: "int")
                .Annotation("SqlServer:Identity", "1, 1");

            migrationBuilder.AlterColumn<int>(
                name: "CourseId",
                table: "Lectures",
                type: "int",
                nullable: true,
                oldClrType: typeof(int),
                oldType: "int");

            migrationBuilder.AlterColumn<int>(
                name: "StudyProgramId",
                table: "CreateCourse",
                type: "int",
                nullable: true,
                oldClrType: typeof(int),
                oldType: "int");

            migrationBuilder.AlterColumn<int>(
                name: "Id",
                table: "Courses",
                type: "int",
                nullable: false,
                oldClrType: typeof(int),
                oldType: "int")
                .Annotation("SqlServer:Identity", "1, 1");

            migrationBuilder.AlterColumn<int>(
                name: "StudyProgramId",
                table: "Courses",
                type: "int",
                nullable: true,
                oldClrType: typeof(int),
                oldType: "int");

            migrationBuilder.AddPrimaryKey(
                name: "PK_Lectures",
                table: "Lectures",
                column: "Id");

            migrationBuilder.AddPrimaryKey(
                name: "PK_Courses",
                table: "Courses",
                column: "Id");

            migrationBuilder.CreateIndex(
                name: "IX_Lectures_CourseId",
                table: "Lectures",
                column: "CourseId");

            migrationBuilder.CreateIndex(
                name: "IX_CreateCourse_StudyProgramId",
                table: "CreateCourse",
                column: "StudyProgramId");

            migrationBuilder.CreateIndex(
                name: "IX_Courses_StudyProgramId",
                table: "Courses",
                column: "StudyProgramId");

            migrationBuilder.AddForeignKey(
                name: "FK_Courses_StudyPrograms_StudyProgramId",
                table: "Courses",
                column: "StudyProgramId",
                principalTable: "StudyPrograms",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_CreateCourse_StudyPrograms_StudyProgramId",
                table: "CreateCourse",
                column: "StudyProgramId",
                principalTable: "StudyPrograms",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_LearningObjects_Lectures_LectureId",
                table: "LearningObjects",
                column: "LectureId",
                principalTable: "Lectures",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_Lectures_Courses_CourseId",
                table: "Lectures",
                column: "CourseId",
                principalTable: "Courses",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_RestrictAccess_Courses_CourseId",
                table: "RestrictAccess",
                column: "CourseId",
                principalTable: "Courses",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

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
