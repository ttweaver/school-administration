using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace WebApplication1.Data.Migrations
{
    /// <inheritdoc />
    public partial class Enrollment : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Assignment_Classroom_CourseId",
                table: "Assignment");

            migrationBuilder.DropForeignKey(
                name: "FK_ClassGrade_Classroom_CourseId",
                table: "ClassGrade");

            migrationBuilder.DropForeignKey(
                name: "FK_Classroom_Teacher_TeacherId",
                table: "Classroom");

            migrationBuilder.DropForeignKey(
                name: "FK_Student_Classroom_CourseId",
                table: "Student");

            migrationBuilder.DropIndex(
                name: "IX_Student_CourseId",
                table: "Student");

            migrationBuilder.DropPrimaryKey(
                name: "PK_Classroom",
                table: "Classroom");

            migrationBuilder.DropColumn(
                name: "ClassroomId",
                table: "Student");

            migrationBuilder.DropColumn(
                name: "CourseId",
                table: "Student");

            migrationBuilder.RenameTable(
                name: "Classroom",
                newName: "Course");

            migrationBuilder.RenameIndex(
                name: "IX_Classroom_TeacherId",
                table: "Course",
                newName: "IX_Course_TeacherId");

            migrationBuilder.AddColumn<int>(
                name: "StudentId",
                table: "Course",
                type: "int",
                nullable: true);

            migrationBuilder.AddPrimaryKey(
                name: "PK_Course",
                table: "Course",
                column: "Id");

            migrationBuilder.CreateTable(
                name: "Enrollment",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    StudentId = table.Column<int>(type: "int", nullable: false),
                    CourseId = table.Column<int>(type: "int", nullable: false),
                    EnrollmentDate = table.Column<DateTime>(type: "datetime2", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Enrollment", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Enrollment_Course_CourseId",
                        column: x => x.CourseId,
                        principalTable: "Course",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_Enrollment_Student_StudentId",
                        column: x => x.StudentId,
                        principalTable: "Student",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_Course_StudentId",
                table: "Course",
                column: "StudentId");

            migrationBuilder.CreateIndex(
                name: "IX_Enrollment_CourseId",
                table: "Enrollment",
                column: "CourseId");

            migrationBuilder.CreateIndex(
                name: "IX_Enrollment_StudentId",
                table: "Enrollment",
                column: "StudentId");

            migrationBuilder.AddForeignKey(
                name: "FK_Assignment_Course_CourseId",
                table: "Assignment",
                column: "CourseId",
                principalTable: "Course",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_ClassGrade_Course_CourseId",
                table: "ClassGrade",
                column: "CourseId",
                principalTable: "Course",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_Course_Student_StudentId",
                table: "Course",
                column: "StudentId",
                principalTable: "Student",
                principalColumn: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_Course_Teacher_TeacherId",
                table: "Course",
                column: "TeacherId",
                principalTable: "Teacher",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Assignment_Course_CourseId",
                table: "Assignment");

            migrationBuilder.DropForeignKey(
                name: "FK_ClassGrade_Course_CourseId",
                table: "ClassGrade");

            migrationBuilder.DropForeignKey(
                name: "FK_Course_Student_StudentId",
                table: "Course");

            migrationBuilder.DropForeignKey(
                name: "FK_Course_Teacher_TeacherId",
                table: "Course");

            migrationBuilder.DropTable(
                name: "Enrollment");

            migrationBuilder.DropPrimaryKey(
                name: "PK_Course",
                table: "Course");

            migrationBuilder.DropIndex(
                name: "IX_Course_StudentId",
                table: "Course");

            migrationBuilder.DropColumn(
                name: "StudentId",
                table: "Course");

            migrationBuilder.RenameTable(
                name: "Course",
                newName: "Classroom");

            migrationBuilder.RenameIndex(
                name: "IX_Course_TeacherId",
                table: "Classroom",
                newName: "IX_Classroom_TeacherId");

            migrationBuilder.AddColumn<int>(
                name: "ClassroomId",
                table: "Student",
                type: "int",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "CourseId",
                table: "Student",
                type: "int",
                nullable: true);

            migrationBuilder.AddPrimaryKey(
                name: "PK_Classroom",
                table: "Classroom",
                column: "Id");

            migrationBuilder.CreateIndex(
                name: "IX_Student_CourseId",
                table: "Student",
                column: "CourseId");

            migrationBuilder.AddForeignKey(
                name: "FK_Assignment_Classroom_CourseId",
                table: "Assignment",
                column: "CourseId",
                principalTable: "Classroom",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_ClassGrade_Classroom_CourseId",
                table: "ClassGrade",
                column: "CourseId",
                principalTable: "Classroom",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_Classroom_Teacher_TeacherId",
                table: "Classroom",
                column: "TeacherId",
                principalTable: "Teacher",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_Student_Classroom_CourseId",
                table: "Student",
                column: "CourseId",
                principalTable: "Classroom",
                principalColumn: "Id");
        }
    }
}
