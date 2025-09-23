using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace WebApplication1.Data.Migrations
{
    /// <inheritdoc />
    public partial class RenameValueToPointsEarned : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Value",
                table: "CourseGrades");

            migrationBuilder.DropColumn(
                name: "Value",
                table: "AssignmentGrades");

            migrationBuilder.AddColumn<int>(
                name: "PointsEarned",
                table: "CourseGrades",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<int>(
                name: "PointsEarned",
                table: "AssignmentGrades",
                type: "int",
                nullable: false,
                defaultValue: 0);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "PointsEarned",
                table: "CourseGrades");

            migrationBuilder.DropColumn(
                name: "PointsEarned",
                table: "AssignmentGrades");

            migrationBuilder.AddColumn<decimal>(
                name: "Value",
                table: "CourseGrades",
                type: "decimal(18,2)",
                nullable: false,
                defaultValue: 0m);

            migrationBuilder.AddColumn<decimal>(
                name: "Value",
                table: "AssignmentGrades",
                type: "decimal(18,2)",
                nullable: false,
                defaultValue: 0m);
        }
    }
}
