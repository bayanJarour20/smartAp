using Microsoft.EntityFrameworkCore.Migrations;

namespace SmartStart.SqlServer.Migrations
{
    public partial class test : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_FacultyPOSUser_AspNetUsers_AppUserId",
                table: "FacultyPOSUser");

            migrationBuilder.DropForeignKey(
                name: "FK_FacultyPOSUser_Faculties_FacultyId",
                table: "FacultyPOSUser");

            migrationBuilder.DropForeignKey(
                name: "FK_SubjectFaculty_Faculties_FacultyId",
                table: "SubjectFaculty");

            migrationBuilder.DropForeignKey(
                name: "FK_SubjectFaculty_Subjects_SubjectId",
                table: "SubjectFaculty");

            migrationBuilder.DropForeignKey(
                name: "FK_SubjectFaculty_Tags_SectionId",
                table: "SubjectFaculty");

            migrationBuilder.DropForeignKey(
                name: "FK_SubjectFaculty_Tags_SemesterId",
                table: "SubjectFaculty");

            migrationBuilder.DropPrimaryKey(
                name: "PK_SubjectFaculty",
                table: "SubjectFaculty");

            migrationBuilder.DropPrimaryKey(
                name: "PK_FacultyPOSUser",
                table: "FacultyPOSUser");

            migrationBuilder.RenameTable(
                name: "SubjectFaculty",
                newName: "SubjectFaculties");

            migrationBuilder.RenameTable(
                name: "FacultyPOSUser",
                newName: "FacultyPOSUsers");

            migrationBuilder.RenameIndex(
                name: "IX_SubjectFaculty_SubjectId",
                table: "SubjectFaculties",
                newName: "IX_SubjectFaculties_SubjectId");

            migrationBuilder.RenameIndex(
                name: "IX_SubjectFaculty_SemesterId",
                table: "SubjectFaculties",
                newName: "IX_SubjectFaculties_SemesterId");

            migrationBuilder.RenameIndex(
                name: "IX_SubjectFaculty_SectionId",
                table: "SubjectFaculties",
                newName: "IX_SubjectFaculties_SectionId");

            migrationBuilder.RenameIndex(
                name: "IX_SubjectFaculty_FacultyId",
                table: "SubjectFaculties",
                newName: "IX_SubjectFaculties_FacultyId");

            migrationBuilder.RenameIndex(
                name: "IX_SubjectFaculty_DateCreated",
                table: "SubjectFaculties",
                newName: "IX_SubjectFaculties_DateCreated");

            migrationBuilder.RenameIndex(
                name: "IX_FacultyPOSUser_FacultyId",
                table: "FacultyPOSUsers",
                newName: "IX_FacultyPOSUsers_FacultyId");

            migrationBuilder.RenameIndex(
                name: "IX_FacultyPOSUser_DateCreated",
                table: "FacultyPOSUsers",
                newName: "IX_FacultyPOSUsers_DateCreated");

            migrationBuilder.RenameIndex(
                name: "IX_FacultyPOSUser_AppUserId",
                table: "FacultyPOSUsers",
                newName: "IX_FacultyPOSUsers_AppUserId");

            migrationBuilder.AddPrimaryKey(
                name: "PK_SubjectFaculties",
                table: "SubjectFaculties",
                column: "Id");

            migrationBuilder.AddPrimaryKey(
                name: "PK_FacultyPOSUsers",
                table: "FacultyPOSUsers",
                column: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_FacultyPOSUsers_AspNetUsers_AppUserId",
                table: "FacultyPOSUsers",
                column: "AppUserId",
                principalTable: "AspNetUsers",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_FacultyPOSUsers_Faculties_FacultyId",
                table: "FacultyPOSUsers",
                column: "FacultyId",
                principalTable: "Faculties",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_SubjectFaculties_Faculties_FacultyId",
                table: "SubjectFaculties",
                column: "FacultyId",
                principalTable: "Faculties",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_SubjectFaculties_Subjects_SubjectId",
                table: "SubjectFaculties",
                column: "SubjectId",
                principalTable: "Subjects",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_SubjectFaculties_Tags_SectionId",
                table: "SubjectFaculties",
                column: "SectionId",
                principalTable: "Tags",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_SubjectFaculties_Tags_SemesterId",
                table: "SubjectFaculties",
                column: "SemesterId",
                principalTable: "Tags",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_FacultyPOSUsers_AspNetUsers_AppUserId",
                table: "FacultyPOSUsers");

            migrationBuilder.DropForeignKey(
                name: "FK_FacultyPOSUsers_Faculties_FacultyId",
                table: "FacultyPOSUsers");

            migrationBuilder.DropForeignKey(
                name: "FK_SubjectFaculties_Faculties_FacultyId",
                table: "SubjectFaculties");

            migrationBuilder.DropForeignKey(
                name: "FK_SubjectFaculties_Subjects_SubjectId",
                table: "SubjectFaculties");

            migrationBuilder.DropForeignKey(
                name: "FK_SubjectFaculties_Tags_SectionId",
                table: "SubjectFaculties");

            migrationBuilder.DropForeignKey(
                name: "FK_SubjectFaculties_Tags_SemesterId",
                table: "SubjectFaculties");

            migrationBuilder.DropPrimaryKey(
                name: "PK_SubjectFaculties",
                table: "SubjectFaculties");

            migrationBuilder.DropPrimaryKey(
                name: "PK_FacultyPOSUsers",
                table: "FacultyPOSUsers");

            migrationBuilder.RenameTable(
                name: "SubjectFaculties",
                newName: "SubjectFaculty");

            migrationBuilder.RenameTable(
                name: "FacultyPOSUsers",
                newName: "FacultyPOSUser");

            migrationBuilder.RenameIndex(
                name: "IX_SubjectFaculties_SubjectId",
                table: "SubjectFaculty",
                newName: "IX_SubjectFaculty_SubjectId");

            migrationBuilder.RenameIndex(
                name: "IX_SubjectFaculties_SemesterId",
                table: "SubjectFaculty",
                newName: "IX_SubjectFaculty_SemesterId");

            migrationBuilder.RenameIndex(
                name: "IX_SubjectFaculties_SectionId",
                table: "SubjectFaculty",
                newName: "IX_SubjectFaculty_SectionId");

            migrationBuilder.RenameIndex(
                name: "IX_SubjectFaculties_FacultyId",
                table: "SubjectFaculty",
                newName: "IX_SubjectFaculty_FacultyId");

            migrationBuilder.RenameIndex(
                name: "IX_SubjectFaculties_DateCreated",
                table: "SubjectFaculty",
                newName: "IX_SubjectFaculty_DateCreated");

            migrationBuilder.RenameIndex(
                name: "IX_FacultyPOSUsers_FacultyId",
                table: "FacultyPOSUser",
                newName: "IX_FacultyPOSUser_FacultyId");

            migrationBuilder.RenameIndex(
                name: "IX_FacultyPOSUsers_DateCreated",
                table: "FacultyPOSUser",
                newName: "IX_FacultyPOSUser_DateCreated");

            migrationBuilder.RenameIndex(
                name: "IX_FacultyPOSUsers_AppUserId",
                table: "FacultyPOSUser",
                newName: "IX_FacultyPOSUser_AppUserId");

            migrationBuilder.AddPrimaryKey(
                name: "PK_SubjectFaculty",
                table: "SubjectFaculty",
                column: "Id");

            migrationBuilder.AddPrimaryKey(
                name: "PK_FacultyPOSUser",
                table: "FacultyPOSUser",
                column: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_FacultyPOSUser_AspNetUsers_AppUserId",
                table: "FacultyPOSUser",
                column: "AppUserId",
                principalTable: "AspNetUsers",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_FacultyPOSUser_Faculties_FacultyId",
                table: "FacultyPOSUser",
                column: "FacultyId",
                principalTable: "Faculties",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_SubjectFaculty_Faculties_FacultyId",
                table: "SubjectFaculty",
                column: "FacultyId",
                principalTable: "Faculties",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_SubjectFaculty_Subjects_SubjectId",
                table: "SubjectFaculty",
                column: "SubjectId",
                principalTable: "Subjects",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_SubjectFaculty_Tags_SectionId",
                table: "SubjectFaculty",
                column: "SectionId",
                principalTable: "Tags",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_SubjectFaculty_Tags_SemesterId",
                table: "SubjectFaculty",
                column: "SemesterId",
                principalTable: "Tags",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }
    }
}
