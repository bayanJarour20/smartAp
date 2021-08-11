using Microsoft.EntityFrameworkCore.Migrations;

namespace SmartStart.SqlServer.Migrations
{
    public partial class ChangePackgeExamToPackgeSubjectNaming : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_PackageExams_Exams_ExamId",
                table: "PackageExams");

            migrationBuilder.DropForeignKey(
                name: "FK_PackageExams_Packages_PackageId",
                table: "PackageExams");

            migrationBuilder.DropForeignKey(
                name: "FK_PackageExams_Subjects_SubjectId",
                table: "PackageExams");

            migrationBuilder.DropPrimaryKey(
                name: "PK_PackageExams",
                table: "PackageExams");

            migrationBuilder.RenameTable(
                name: "PackageExams",
                newName: "PackageSubjects");

            migrationBuilder.RenameIndex(
                name: "IX_PackageExams_SubjectId",
                table: "PackageSubjects",
                newName: "IX_PackageSubjects_SubjectId");

            migrationBuilder.RenameIndex(
                name: "IX_PackageExams_PackageId",
                table: "PackageSubjects",
                newName: "IX_PackageSubjects_PackageId");

            migrationBuilder.RenameIndex(
                name: "IX_PackageExams_ExamId",
                table: "PackageSubjects",
                newName: "IX_PackageSubjects_ExamId");

            migrationBuilder.RenameIndex(
                name: "IX_PackageExams_DateCreated",
                table: "PackageSubjects",
                newName: "IX_PackageSubjects_DateCreated");

            migrationBuilder.AddPrimaryKey(
                name: "PK_PackageSubjects",
                table: "PackageSubjects",
                column: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_PackageSubjects_Exams_ExamId",
                table: "PackageSubjects",
                column: "ExamId",
                principalTable: "Exams",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_PackageSubjects_Packages_PackageId",
                table: "PackageSubjects",
                column: "PackageId",
                principalTable: "Packages",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_PackageSubjects_Subjects_SubjectId",
                table: "PackageSubjects",
                column: "SubjectId",
                principalTable: "Subjects",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_PackageSubjects_Exams_ExamId",
                table: "PackageSubjects");

            migrationBuilder.DropForeignKey(
                name: "FK_PackageSubjects_Packages_PackageId",
                table: "PackageSubjects");

            migrationBuilder.DropForeignKey(
                name: "FK_PackageSubjects_Subjects_SubjectId",
                table: "PackageSubjects");

            migrationBuilder.DropPrimaryKey(
                name: "PK_PackageSubjects",
                table: "PackageSubjects");

            migrationBuilder.RenameTable(
                name: "PackageSubjects",
                newName: "PackageExams");

            migrationBuilder.RenameIndex(
                name: "IX_PackageSubjects_SubjectId",
                table: "PackageExams",
                newName: "IX_PackageExams_SubjectId");

            migrationBuilder.RenameIndex(
                name: "IX_PackageSubjects_PackageId",
                table: "PackageExams",
                newName: "IX_PackageExams_PackageId");

            migrationBuilder.RenameIndex(
                name: "IX_PackageSubjects_ExamId",
                table: "PackageExams",
                newName: "IX_PackageExams_ExamId");

            migrationBuilder.RenameIndex(
                name: "IX_PackageSubjects_DateCreated",
                table: "PackageExams",
                newName: "IX_PackageExams_DateCreated");

            migrationBuilder.AddPrimaryKey(
                name: "PK_PackageExams",
                table: "PackageExams",
                column: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_PackageExams_Exams_ExamId",
                table: "PackageExams",
                column: "ExamId",
                principalTable: "Exams",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_PackageExams_Packages_PackageId",
                table: "PackageExams",
                column: "PackageId",
                principalTable: "Packages",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_PackageExams_Subjects_SubjectId",
                table: "PackageExams",
                column: "SubjectId",
                principalTable: "Subjects",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
