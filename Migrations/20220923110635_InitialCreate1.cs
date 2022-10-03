using Microsoft.EntityFrameworkCore.Migrations;

namespace MMUniGraduation.Migrations
{
    public partial class InitialCreate1 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Courses_Lectors_LectorID",
                table: "Courses");

            migrationBuilder.DropForeignKey(
                name: "FK_Lectures_Lectors_LectorID",
                table: "Lectures");

            migrationBuilder.DropForeignKey(
                name: "FK_RestrictAccess_SkippingAssignment_SkippingAssignmentId",
                table: "RestrictAccess");

            migrationBuilder.DropForeignKey(
                name: "FK_RestrictAccess_Students_StudentId",
                table: "RestrictAccess");

            migrationBuilder.DropTable(
                name: "SkippingAssignment");

            migrationBuilder.DropIndex(
                name: "IX_RestrictAccess_SkippingAssignmentId",
                table: "RestrictAccess");

            migrationBuilder.DropIndex(
                name: "IX_RestrictAccess_StudentId",
                table: "RestrictAccess");

            migrationBuilder.DropIndex(
                name: "IX_Lectures_LectorID",
                table: "Lectures");

            migrationBuilder.DropIndex(
                name: "IX_Courses_LectorID",
                table: "Courses");

            migrationBuilder.DropColumn(
                name: "SkippingAssignmentId",
                table: "RestrictAccess");

            migrationBuilder.DropColumn(
                name: "StudentId",
                table: "RestrictAccess");

            migrationBuilder.DropColumn(
                name: "LectorID",
                table: "Lectures");

            migrationBuilder.DropColumn(
                name: "LectorID",
                table: "Courses");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "SkippingAssignmentId",
                table: "RestrictAccess",
                type: "int",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "StudentId",
                table: "RestrictAccess",
                type: "int",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "LectorID",
                table: "Lectures",
                type: "int",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "LectorID",
                table: "Courses",
                type: "int",
                nullable: true);

            migrationBuilder.CreateTable(
                name: "SkippingAssignment",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Assignment = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    CourseId = table.Column<int>(type: "int", nullable: false),
                    Grade = table.Column<decimal>(type: "decimal(18,2)", nullable: false),
                    IsPassed = table.Column<bool>(type: "bit", nullable: false),
                    Name = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    NexrCourseSignature = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    NextSkippingSignature = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    ParetntID = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Signature = table.Column<string>(type: "nvarchar(max)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_SkippingAssignment", x => x.Id);
                });

            migrationBuilder.CreateIndex(
                name: "IX_RestrictAccess_SkippingAssignmentId",
                table: "RestrictAccess",
                column: "SkippingAssignmentId");

            migrationBuilder.CreateIndex(
                name: "IX_RestrictAccess_StudentId",
                table: "RestrictAccess",
                column: "StudentId");

            migrationBuilder.CreateIndex(
                name: "IX_Lectures_LectorID",
                table: "Lectures",
                column: "LectorID");

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

            migrationBuilder.AddForeignKey(
                name: "FK_Lectures_Lectors_LectorID",
                table: "Lectures",
                column: "LectorID",
                principalTable: "Lectors",
                principalColumn: "ID",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_RestrictAccess_SkippingAssignment_SkippingAssignmentId",
                table: "RestrictAccess",
                column: "SkippingAssignmentId",
                principalTable: "SkippingAssignment",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_RestrictAccess_Students_StudentId",
                table: "RestrictAccess",
                column: "StudentId",
                principalTable: "Students",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }
    }
}
