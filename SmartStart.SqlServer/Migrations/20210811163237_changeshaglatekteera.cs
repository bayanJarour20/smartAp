using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace SmartStart.SqlServer.Migrations
{
    public partial class changeshaglatekteera : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "PackageSubjects");

            migrationBuilder.AddColumn<byte>(
                name: "Type",
                table: "Codes",
                type: "tinyint",
                nullable: false,
                defaultValue: (byte)0);

            migrationBuilder.CreateTable(
                name: "PackageSubjectFaculties",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    Price = table.Column<double>(type: "float", nullable: false),
                    PackageId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    SubjectFacultyId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    ExamId = table.Column<Guid>(type: "uniqueidentifier", nullable: true),
                    DateDeleted = table.Column<DateTime>(type: "datetime2", nullable: true),
                    DateCreated = table.Column<DateTime>(type: "datetime2", nullable: false),
                    DateUpdated = table.Column<DateTime>(type: "datetime2", nullable: true),
                    DeletedBy = table.Column<Guid>(type: "uniqueidentifier", nullable: true),
                    CreatedBy = table.Column<Guid>(type: "uniqueidentifier", nullable: true),
                    UpdatedBy = table.Column<Guid>(type: "uniqueidentifier", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_PackageSubjectFaculties", x => x.Id);
                    table.ForeignKey(
                        name: "FK_PackageSubjectFaculties_Exams_ExamId",
                        column: x => x.ExamId,
                        principalTable: "Exams",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_PackageSubjectFaculties_Packages_PackageId",
                        column: x => x.PackageId,
                        principalTable: "Packages",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_PackageSubjectFaculties_SubjectFaculties_SubjectFacultyId",
                        column: x => x.SubjectFacultyId,
                        principalTable: "SubjectFaculties",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_PackageSubjectFaculties_DateCreated",
                table: "PackageSubjectFaculties",
                column: "DateCreated");

            migrationBuilder.CreateIndex(
                name: "IX_PackageSubjectFaculties_ExamId",
                table: "PackageSubjectFaculties",
                column: "ExamId");

            migrationBuilder.CreateIndex(
                name: "IX_PackageSubjectFaculties_PackageId",
                table: "PackageSubjectFaculties",
                column: "PackageId");

            migrationBuilder.CreateIndex(
                name: "IX_PackageSubjectFaculties_SubjectFacultyId",
                table: "PackageSubjectFaculties",
                column: "SubjectFacultyId");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "PackageSubjectFaculties");

            migrationBuilder.DropColumn(
                name: "Type",
                table: "Codes");

            migrationBuilder.CreateTable(
                name: "PackageSubjects",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    CreatedBy = table.Column<Guid>(type: "uniqueidentifier", nullable: true),
                    DateCreated = table.Column<DateTime>(type: "datetime2", nullable: false),
                    DateDeleted = table.Column<DateTime>(type: "datetime2", nullable: true),
                    DateUpdated = table.Column<DateTime>(type: "datetime2", nullable: true),
                    DeletedBy = table.Column<Guid>(type: "uniqueidentifier", nullable: true),
                    ExamId = table.Column<Guid>(type: "uniqueidentifier", nullable: true),
                    PackageId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    Price = table.Column<double>(type: "float", nullable: false),
                    SubjectId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    UpdatedBy = table.Column<Guid>(type: "uniqueidentifier", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_PackageSubjects", x => x.Id);
                    table.ForeignKey(
                        name: "FK_PackageSubjects_Exams_ExamId",
                        column: x => x.ExamId,
                        principalTable: "Exams",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_PackageSubjects_Packages_PackageId",
                        column: x => x.PackageId,
                        principalTable: "Packages",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_PackageSubjects_Subjects_SubjectId",
                        column: x => x.SubjectId,
                        principalTable: "Subjects",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_PackageSubjects_DateCreated",
                table: "PackageSubjects",
                column: "DateCreated");

            migrationBuilder.CreateIndex(
                name: "IX_PackageSubjects_ExamId",
                table: "PackageSubjects",
                column: "ExamId");

            migrationBuilder.CreateIndex(
                name: "IX_PackageSubjects_PackageId",
                table: "PackageSubjects",
                column: "PackageId");

            migrationBuilder.CreateIndex(
                name: "IX_PackageSubjects_SubjectId",
                table: "PackageSubjects",
                column: "SubjectId");
        }
    }
}
