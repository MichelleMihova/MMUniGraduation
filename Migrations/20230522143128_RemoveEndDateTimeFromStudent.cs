using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace MMUniGraduation.Migrations
{
    public partial class RemoveEndDateTimeFromStudent : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "DateOfBirth",
                table: "Students");

            migrationBuilder.DropColumn(
                name: "EndDateTime",
                table: "Students");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<DateTime>(
                name: "DateOfBirth",
                table: "Students",
                type: "datetime2",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified));

            migrationBuilder.AddColumn<DateTime>(
                name: "EndDateTime",
                table: "Students",
                type: "datetime2",
                nullable: true);
        }
    }
}
