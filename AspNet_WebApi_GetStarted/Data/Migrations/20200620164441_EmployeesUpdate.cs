using Microsoft.EntityFrameworkCore.Migrations;

namespace AspNet_WebApi_GetStarted.Data.Migrations
{
    public partial class EmployeesUpdate : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "Secret",
                table: "Employees",
                nullable: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Secret",
                table: "Employees");
        }
    }
}
