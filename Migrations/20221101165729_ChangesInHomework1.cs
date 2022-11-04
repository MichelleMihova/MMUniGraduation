using Microsoft.EntityFrameworkCore.Migrations;

namespace MMUniGraduation.Migrations
{
    public partial class ChangesInHomework1 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Homework_Lectures_LectureId",
                table: "Homework");

            migrationBuilder.DropPrimaryKey(
                name: "PK_Homework",
                table: "Homework");

            migrationBuilder.RenameTable(
                name: "Homework",
                newName: "Homeworks");

            migrationBuilder.RenameIndex(
                name: "IX_Homework_LectureId",
                table: "Homeworks",
                newName: "IX_Homeworks_LectureId");

            migrationBuilder.AddPrimaryKey(
                name: "PK_Homeworks",
                table: "Homeworks",
                column: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_Homeworks_Lectures_LectureId",
                table: "Homeworks",
                column: "LectureId",
                principalTable: "Lectures",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Homeworks_Lectures_LectureId",
                table: "Homeworks");

            migrationBuilder.DropPrimaryKey(
                name: "PK_Homeworks",
                table: "Homeworks");

            migrationBuilder.RenameTable(
                name: "Homeworks",
                newName: "Homework");

            migrationBuilder.RenameIndex(
                name: "IX_Homeworks_LectureId",
                table: "Homework",
                newName: "IX_Homework_LectureId");

            migrationBuilder.AddPrimaryKey(
                name: "PK_Homework",
                table: "Homework",
                column: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_Homework_Lectures_LectureId",
                table: "Homework",
                column: "LectureId",
                principalTable: "Lectures",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }
    }
}
