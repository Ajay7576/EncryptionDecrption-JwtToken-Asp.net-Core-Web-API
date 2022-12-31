using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Passwordmanager.Migrations
{
    public partial class addenckeycol : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
           

            migrationBuilder.AddColumn<string>(
                name: "Encryptionkey",
                table: "PasswordManagers",
                type: "nvarchar(max)",
                nullable: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Encryptionkey",
                table: "PasswordManagers");

            migrationBuilder.CreateTable(
                name: "Encryptions",
                columns: table => new
                {
                    UserId = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Encryptionkey = table.Column<string>(type: "nvarchar(max)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Encryptions", x => x.UserId);
                });
        }
    }
}
