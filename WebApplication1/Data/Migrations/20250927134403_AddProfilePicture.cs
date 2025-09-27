using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace WebApplication1.Data.Migrations
{
    /// <inheritdoc />
    public partial class AddProfilePicture : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<byte[]>(
                name: "ProfilePicture",
                table: "Students",
                type: "varbinary(max)",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "ProfilePictureContentType",
                table: "Students",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_AssignmentScores_StudentId",
                table: "AssignmentScores",
                column: "StudentId");

            migrationBuilder.AddForeignKey(
                name: "FK_AssignmentScores_Students_StudentId",
                table: "AssignmentScores",
                column: "StudentId",
                principalTable: "Students",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_AssignmentScores_Students_StudentId",
                table: "AssignmentScores");

            migrationBuilder.DropIndex(
                name: "IX_AssignmentScores_StudentId",
                table: "AssignmentScores");

            migrationBuilder.DropColumn(
                name: "ProfilePicture",
                table: "Students");

            migrationBuilder.DropColumn(
                name: "ProfilePictureContentType",
                table: "Students");
        }
    }
}
