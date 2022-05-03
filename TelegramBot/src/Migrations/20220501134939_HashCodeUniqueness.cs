using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace DesktopApp.Migrations;

public partial class HashCodeUniqueness : Migration
{
    protected override void Up(MigrationBuilder migrationBuilder)
    {
        migrationBuilder.DropIndex(
            name: "IX_Songs_FileUniqueId",
            table: "Songs");

        migrationBuilder.CreateIndex(
            name: "IX_Songs_FileUniqueId_HashCode",
            table: "Songs",
            columns: new[] { "FileUniqueId", "HashCode" },
            unique: true);
    }

    protected override void Down(MigrationBuilder migrationBuilder)
    {
        migrationBuilder.DropIndex(
            name: "IX_Songs_FileUniqueId_HashCode",
            table: "Songs");

        migrationBuilder.CreateIndex(
            name: "IX_Songs_FileUniqueId",
            table: "Songs",
            column: "FileUniqueId",
            unique: true);
    }
}