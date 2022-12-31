using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Passwordmanager.Migrations
{
    public partial class addnamecol : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "Name",
                table: "PasswordManagers",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Name",
                table: "PasswordManagers");
        }
    }
}
