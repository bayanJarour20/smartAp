using Microsoft.EntityFrameworkCore.Migrations;

namespace SmartStart.SqlServer.Migrations
{
    public partial class addpriceSubjectFaculty : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<double>(
                name: "Price",
                table: "SubjectFaculties",
                type: "float",
                nullable: false,
                defaultValue: 0.0);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Price",
                table: "SubjectFaculties");
        }
    }
}
