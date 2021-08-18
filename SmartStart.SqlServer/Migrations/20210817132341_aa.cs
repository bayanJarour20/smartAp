using Microsoft.EntityFrameworkCore.Migrations;

namespace SmartStart.SqlServer.Migrations
{
    public partial class aa : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_ExamDocument_Documents_DocumentId",
                table: "ExamDocument");

            migrationBuilder.DropForeignKey(
                name: "FK_ExamDocument_Exams_ExamId",
                table: "ExamDocument");

            migrationBuilder.DropPrimaryKey(
                name: "PK_ExamDocument",
                table: "ExamDocument");

            migrationBuilder.RenameTable(
                name: "ExamDocument",
                newName: "ExamDocuments");

            migrationBuilder.RenameIndex(
                name: "IX_ExamDocument_ExamId",
                table: "ExamDocuments",
                newName: "IX_ExamDocuments_ExamId");

            migrationBuilder.RenameIndex(
                name: "IX_ExamDocument_DocumentId",
                table: "ExamDocuments",
                newName: "IX_ExamDocuments_DocumentId");

            migrationBuilder.RenameIndex(
                name: "IX_ExamDocument_DateCreated",
                table: "ExamDocuments",
                newName: "IX_ExamDocuments_DateCreated");

            migrationBuilder.AddColumn<string>(
                name: "Note",
                table: "ExamDocuments",
                type: "nvarchar(1024)",
                nullable: true);

            migrationBuilder.AddPrimaryKey(
                name: "PK_ExamDocuments",
                table: "ExamDocuments",
                column: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_ExamDocuments_Documents_DocumentId",
                table: "ExamDocuments",
                column: "DocumentId",
                principalTable: "Documents",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_ExamDocuments_Exams_ExamId",
                table: "ExamDocuments",
                column: "ExamId",
                principalTable: "Exams",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_ExamDocuments_Documents_DocumentId",
                table: "ExamDocuments");

            migrationBuilder.DropForeignKey(
                name: "FK_ExamDocuments_Exams_ExamId",
                table: "ExamDocuments");

            migrationBuilder.DropPrimaryKey(
                name: "PK_ExamDocuments",
                table: "ExamDocuments");

            migrationBuilder.DropColumn(
                name: "Note",
                table: "ExamDocuments");

            migrationBuilder.RenameTable(
                name: "ExamDocuments",
                newName: "ExamDocument");

            migrationBuilder.RenameIndex(
                name: "IX_ExamDocuments_ExamId",
                table: "ExamDocument",
                newName: "IX_ExamDocument_ExamId");

            migrationBuilder.RenameIndex(
                name: "IX_ExamDocuments_DocumentId",
                table: "ExamDocument",
                newName: "IX_ExamDocument_DocumentId");

            migrationBuilder.RenameIndex(
                name: "IX_ExamDocuments_DateCreated",
                table: "ExamDocument",
                newName: "IX_ExamDocument_DateCreated");

            migrationBuilder.AddPrimaryKey(
                name: "PK_ExamDocument",
                table: "ExamDocument",
                column: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_ExamDocument_Documents_DocumentId",
                table: "ExamDocument",
                column: "DocumentId",
                principalTable: "Documents",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_ExamDocument_Exams_ExamId",
                table: "ExamDocument",
                column: "ExamId",
                principalTable: "Exams",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
