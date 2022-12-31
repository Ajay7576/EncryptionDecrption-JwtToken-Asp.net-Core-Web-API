using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Passwordmanager.Migrations
{
    public partial class addsomecolumninuserinfo : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
           
            migrationBuilder.RenameColumn(
                name: "CreatedDate",
                table: "UserInfos",
                newName: "CreatedAt");

            migrationBuilder.AddColumn<string>(
                name: "RefreshToken",
                table: "UserInfos",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "RefreshTokenExpiryTime",
                table: "UserInfos",
                type: "nvarchar(max)",
                nullable: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "RefreshToken",
                table: "UserInfos");

            migrationBuilder.DropColumn(
                name: "RefreshTokenExpiryTime",
                table: "UserInfos");

            migrationBuilder.RenameColumn(
                name: "CreatedAt",
                table: "UserInfos",
                newName: "CreatedDate");

            migrationBuilder.CreateTable(
                name: "UserRefreshTokens",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    CreatedAt = table.Column<DateTime>(type: "datetime2", nullable: false),
                    RefreshToken = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    RefreshTokenExpiryTime = table.Column<string>(type: "nvarchar(max)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_UserRefreshTokens", x => x.Id);
                });
        }
    }
}
