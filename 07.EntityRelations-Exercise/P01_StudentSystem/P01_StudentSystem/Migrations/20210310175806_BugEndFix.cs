using Microsoft.EntityFrameworkCore.Migrations;

namespace P01_StudentSystem.Migrations
{
    public partial class BugEndFix : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_HomeworkSubmisions_Courses_CourseId",
                table: "HomeworkSubmisions");

            migrationBuilder.DropForeignKey(
                name: "FK_HomeworkSubmisions_Students_StudentId",
                table: "HomeworkSubmisions");

            migrationBuilder.DropPrimaryKey(
                name: "PK_HomeworkSubmisions",
                table: "HomeworkSubmisions");

            migrationBuilder.EnsureSchema(
                name: "dbo");

            migrationBuilder.RenameTable(
                name: "HomeworkSubmisions",
                newName: "Homework",
                newSchema: "dbo");

            migrationBuilder.RenameIndex(
                name: "IX_HomeworkSubmisions_StudentId",
                schema: "dbo",
                table: "Homework",
                newName: "IX_Homework_StudentId");

            migrationBuilder.RenameIndex(
                name: "IX_HomeworkSubmisions_CourseId",
                schema: "dbo",
                table: "Homework",
                newName: "IX_Homework_CourseId");

            migrationBuilder.AddPrimaryKey(
                name: "PK_Homework",
                schema: "dbo",
                table: "Homework",
                column: "HomeworkId");

            migrationBuilder.AddForeignKey(
                name: "FK_Homework_Courses_CourseId",
                schema: "dbo",
                table: "Homework",
                column: "CourseId",
                principalTable: "Courses",
                principalColumn: "CourseId",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_Homework_Students_StudentId",
                schema: "dbo",
                table: "Homework",
                column: "StudentId",
                principalTable: "Students",
                principalColumn: "StudentId",
                onDelete: ReferentialAction.Cascade);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Homework_Courses_CourseId",
                schema: "dbo",
                table: "Homework");

            migrationBuilder.DropForeignKey(
                name: "FK_Homework_Students_StudentId",
                schema: "dbo",
                table: "Homework");

            migrationBuilder.DropPrimaryKey(
                name: "PK_Homework",
                schema: "dbo",
                table: "Homework");

            migrationBuilder.RenameTable(
                name: "Homework",
                schema: "dbo",
                newName: "HomeworkSubmisions");

            migrationBuilder.RenameIndex(
                name: "IX_Homework_StudentId",
                table: "HomeworkSubmisions",
                newName: "IX_HomeworkSubmisions_StudentId");

            migrationBuilder.RenameIndex(
                name: "IX_Homework_CourseId",
                table: "HomeworkSubmisions",
                newName: "IX_HomeworkSubmisions_CourseId");

            migrationBuilder.AddPrimaryKey(
                name: "PK_HomeworkSubmisions",
                table: "HomeworkSubmisions",
                column: "HomeworkId");

            migrationBuilder.AddForeignKey(
                name: "FK_HomeworkSubmisions_Courses_CourseId",
                table: "HomeworkSubmisions",
                column: "CourseId",
                principalTable: "Courses",
                principalColumn: "CourseId",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_HomeworkSubmisions_Students_StudentId",
                table: "HomeworkSubmisions",
                column: "StudentId",
                principalTable: "Students",
                principalColumn: "StudentId",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
