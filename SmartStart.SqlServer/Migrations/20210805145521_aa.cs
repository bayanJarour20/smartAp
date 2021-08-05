using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace SmartStart.SqlServer.Migrations
{
    public partial class aa : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "GenerationStamp",
                table: "AspNetUsers",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.CreateTable(
                name: "FacultyPOSUser",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    DefaultSelected = table.Column<bool>(type: "bit", nullable: false),
                    FacultyId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
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
                    table.PrimaryKey("PK_FacultyPOSUser", x => x.Id);
                    table.ForeignKey(
                        name: "FK_FacultyPOSUser_AspNetUsers_AppUserId",
                        column: x => x.AppUserId,
                        principalTable: "AspNetUsers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_FacultyPOSUser_Faculties_FacultyId",
                        column: x => x.FacultyId,
                        principalTable: "Faculties",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_FacultyPOSUser_AppUserId",
                table: "FacultyPOSUser",
                column: "AppUserId");

            migrationBuilder.CreateIndex(
                name: "IX_FacultyPOSUser_DateCreated",
                table: "FacultyPOSUser",
                column: "DateCreated");

            migrationBuilder.CreateIndex(
                name: "IX_FacultyPOSUser_FacultyId",
                table: "FacultyPOSUser",
                column: "FacultyId");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "FacultyPOSUser");

            migrationBuilder.DropColumn(
                name: "GenerationStamp",
                table: "AspNetUsers");
        }
    }
}
