using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace SmartStart.SqlServer.Migrations
{
    public partial class addSubjectFaculty : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Subjects_Faculties_FacultyId",
                table: "Subjects");

            migrationBuilder.DropIndex(
                name: "IX_Subjects_FacultyId",
                table: "Subjects");

            migrationBuilder.DropColumn(
                name: "FacultyId",
                table: "Subjects");

            migrationBuilder.DropColumn(
                name: "Year",
                table: "Subjects");

            migrationBuilder.CreateTable(
                name: "SubjectFaculty",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    SubjectId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    FacultyId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    Year = table.Column<short>(type: "smallint", nullable: false),
                    SemesterId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    SectionId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    DateDeleted = table.Column<DateTime>(type: "datetime2", nullable: true),
                    DateCreated = table.Column<DateTime>(type: "datetime2", nullable: false),
                    DateUpdated = table.Column<DateTime>(type: "datetime2", nullable: true),
                    DeletedBy = table.Column<Guid>(type: "uniqueidentifier", nullable: true),
                    CreatedBy = table.Column<Guid>(type: "uniqueidentifier", nullable: true),
                    UpdatedBy = table.Column<Guid>(type: "uniqueidentifier", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_SubjectFaculty", x => x.Id);
                    table.ForeignKey(
                        name: "FK_SubjectFaculty_Faculties_FacultyId",
                        column: x => x.FacultyId,
                        principalTable: "Faculties",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_SubjectFaculty_Subjects_SubjectId",
                        column: x => x.SubjectId,
                        principalTable: "Subjects",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_SubjectFaculty_Tags_SectionId",
                        column: x => x.SectionId,
                        principalTable: "Tags",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_SubjectFaculty_Tags_SemesterId",
                        column: x => x.SemesterId,
                        principalTable: "Tags",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateIndex(
                name: "IX_SubjectFaculty_DateCreated",
                table: "SubjectFaculty",
                column: "DateCreated");

            migrationBuilder.CreateIndex(
                name: "IX_SubjectFaculty_FacultyId",
                table: "SubjectFaculty",
                column: "FacultyId");

            migrationBuilder.CreateIndex(
                name: "IX_SubjectFaculty_SectionId",
                table: "SubjectFaculty",
                column: "SectionId");

            migrationBuilder.CreateIndex(
                name: "IX_SubjectFaculty_SemesterId",
                table: "SubjectFaculty",
                column: "SemesterId");

            migrationBuilder.CreateIndex(
                name: "IX_SubjectFaculty_SubjectId",
                table: "SubjectFaculty",
                column: "SubjectId");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "SubjectFaculty");

            migrationBuilder.AddColumn<Guid>(
                name: "FacultyId",
                table: "Subjects",
                type: "uniqueidentifier",
                nullable: false,
                defaultValue: new Guid("00000000-0000-0000-0000-000000000000"));

            migrationBuilder.AddColumn<short>(
                name: "Year",
                table: "Subjects",
                type: "smallint",
                nullable: false,
                defaultValue: (short)0);

            migrationBuilder.CreateIndex(
                name: "IX_Subjects_FacultyId",
                table: "Subjects",
                column: "FacultyId");

            migrationBuilder.AddForeignKey(
                name: "FK_Subjects_Faculties_FacultyId",
                table: "Subjects",
                column: "FacultyId",
                principalTable: "Faculties",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
