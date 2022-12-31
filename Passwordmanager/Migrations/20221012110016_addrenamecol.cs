using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Passwordmanager.Migrations
{
    public partial class addrenamecol : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "modified",
                table: "PasswordManagers",
                newName: "CreatedOn");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "CreatedOn",
                table: "PasswordManagers",
                newName: "modified");
        }
    }
}
