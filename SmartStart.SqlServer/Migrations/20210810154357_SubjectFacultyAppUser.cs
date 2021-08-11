using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace SmartStart.SqlServer.Migrations
{
    public partial class SubjectFacultyAppUser : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "SubjectAppUsers");

            migrationBuilder.CreateTable(
                name: "SubjectFacultyAppUsers",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    DefaultSelected = table.Column<bool>(type: "bit", nullable: false),
                    SubjectFacultyId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    AppUserId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    DateDeleted = table.Column<DateTime>(type: "datetime2", nullable: true),
                    DateCreated = table.Column<DateTime>(type: "datetime2", nullable: false),
                    DateUpdated = table.Column<DateTime>(type: "datetime2", nullable: true),
                    DeletedBy = table.Column<Guid>(type: "uniqueidentifier", nullable: true),
                    CreatedBy = table.Column<Guid>(type: "uniqueidentifier", nullable: true),
                    UpdatedBy = table.Column<Guid>(type: "uniqueidentifier", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_SubjectFacultyAppUsers", x => x.Id);
                    table.ForeignKey(
                        name: "FK_SubjectFacultyAppUsers_AspNetUsers_AppUserId",
                        column: x => x.AppUserId,
                        principalTable: "AspNetUsers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_SubjectFacultyAppUsers_SubjectFaculty_SubjectFacultyId",
                        column: x => x.SubjectFacultyId,
                        principalTable: "SubjectFaculty",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_SubjectFacultyAppUsers_AppUserId",
                table: "SubjectFacultyAppUsers",
                column: "AppUserId");

            migrationBuilder.CreateIndex(
                name: "IX_SubjectFacultyAppUsers_DateCreated",
                table: "SubjectFacultyAppUsers",
                column: "DateCreated");

            migrationBuilder.CreateIndex(
                name: "IX_SubjectFacultyAppUsers_SubjectFacultyId",
                table: "SubjectFacultyAppUsers",
                column: "SubjectFacultyId");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "SubjectFacultyAppUsers");

            migrationBuilder.CreateTable(
                name: "SubjectAppUsers",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    AppUserId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    CreatedBy = table.Column<Guid>(type: "uniqueidentifier", nullable: true),
                    DateCreated = table.Column<DateTime>(type: "datetime2", nullable: false),
                    DateDeleted = table.Column<DateTime>(type: "datetime2", nullable: true),
                    DateUpdated = table.Column<DateTime>(type: "datetime2", nullable: true),
                    DefaultSelected = table.Column<bool>(type: "bit", nullable: false),
                    DeletedBy = table.Column<Guid>(type: "uniqueidentifier", nullable: true),
                    SubjectId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    UpdatedBy = table.Column<Guid>(type: "uniqueidentifier", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_SubjectAppUsers", x => x.Id);
                    table.ForeignKey(
                        name: "FK_SubjectAppUsers_AspNetUsers_AppUserId",
                        column: x => x.AppUserId,
                        principalTable: "AspNetUsers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_SubjectAppUsers_Subjects_SubjectId",
                        column: x => x.SubjectId,
                        principalTable: "Subjects",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_SubjectAppUsers_AppUserId",
                table: "SubjectAppUsers",
                column: "AppUserId");

            migrationBuilder.CreateIndex(
                name: "IX_SubjectAppUsers_DateCreated",
                table: "SubjectAppUsers",
                column: "DateCreated");

            migrationBuilder.CreateIndex(
                name: "IX_SubjectAppUsers_SubjectId",
                table: "SubjectAppUsers",
                column: "SubjectId");
        }
    }
}
