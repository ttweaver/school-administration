using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace WebApplication1.Data.Migrations
{
    /// <inheritdoc />
    public partial class RenameClassroomToCourse : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Student_Classroom_ClassroomId",
                table: "Student");

            migrationBuilder.DropIndex(
                name: "IX_Student_ClassroomId",
                table: "Student");

            migrationBuilder.AddColumn<int>(
                name: "CourseId",
                table: "Student",
                type: "int",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_Student_CourseId",
                table: "Student",
                column: "CourseId");

            migrationBuilder.AddForeignKey(
                name: "FK_Student_Classroom_CourseId",
                table: "Student",
                column: "CourseId",
                principalTable: "Classroom",
                principalColumn: "Id");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Student_Classroom_CourseId",
                table: "Student");

            migrationBuilder.DropIndex(
                name: "IX_Student_CourseId",
                table: "Student");

            migrationBuilder.DropColumn(
                name: "CourseId",
                table: "Student");

            migrationBuilder.CreateIndex(
                name: "IX_Student_ClassroomId",
                table: "Student",
                column: "ClassroomId");

            migrationBuilder.AddForeignKey(
                name: "FK_Student_Classroom_ClassroomId",
                table: "Student",
                column: "ClassroomId",
                principalTable: "Classroom",
                principalColumn: "Id");
        }
    }
}
