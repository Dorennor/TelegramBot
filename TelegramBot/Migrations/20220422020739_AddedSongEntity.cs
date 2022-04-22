using Microsoft.EntityFrameworkCore.Migrations;
using System;

#nullable disable

namespace DesktopApp.Migrations;

public partial class AddedSongEntity : Migration
{
    protected override void Up(MigrationBuilder migrationBuilder)
    {
        migrationBuilder.CreateTable(
            name: "Songs",
            columns: table => new
            {
                Key = table.Column<int>(type: "int", nullable: false)
                    .Annotation("SqlServer:Identity", "1, 1"),
                FileId = table.Column<string>(type: "nvarchar(max)", nullable: false),
                FileUniqueId = table.Column<string>(type: "nvarchar(450)", nullable: false),
                FileName = table.Column<string>(type: "nvarchar(max)", nullable: false),
                Duration = table.Column<int>(type: "int", nullable: false),
                AddedDateTime = table.Column<DateTime>(type: "datetime2", nullable: false),
                Title = table.Column<string>(type: "nvarchar(max)", nullable: true),
                Artist = table.Column<string>(type: "nvarchar(max)", nullable: true),
                Album = table.Column<string>(type: "nvarchar(max)", nullable: true),
                Year = table.Column<int>(type: "int", nullable: true),
                Rating = table.Column<int>(type: "int", nullable: true),
                Performers = table.Column<string>(type: "nvarchar(max)", nullable: true),
                Genres = table.Column<string>(type: "nvarchar(max)", nullable: true),
                Tags = table.Column<string>(type: "nvarchar(max)", nullable: true)
            },
            constraints: table =>
            {
                table.PrimaryKey("PK_Songs", x => x.Key);
            });

        migrationBuilder.CreateIndex(
            name: "IX_Songs_FileUniqueId",
            table: "Songs",
            column: "FileUniqueId",
            unique: true);
    }

    protected override void Down(MigrationBuilder migrationBuilder)
    {
        migrationBuilder.DropTable(
            name: "Songs");
    }
}