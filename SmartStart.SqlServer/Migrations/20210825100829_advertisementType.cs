using Microsoft.EntityFrameworkCore.Migrations;

namespace SmartStart.SqlServer.Migrations
{
    public partial class advertisementType : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<byte>(
                name: "Type",
                table: "Advertisements",
                type: "tinyint",
                nullable: false,
                defaultValue: (byte)0);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Type",
                table: "Advertisements");
        }
    }
}
