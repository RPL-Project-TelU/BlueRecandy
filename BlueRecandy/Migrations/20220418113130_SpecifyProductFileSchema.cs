using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace BlueRecandy.Migrations
{
    public partial class SpecifyProductFileSchema : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "SourceFile",
                table: "Products",
                newName: "SourceFileContents");

            migrationBuilder.AddColumn<string>(
                name: "SourceFileContentType",
                table: "Products",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<string>(
                name: "SourceFileName",
                table: "Products",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "SourceFileContentType",
                table: "Products");

            migrationBuilder.DropColumn(
                name: "SourceFileName",
                table: "Products");

            migrationBuilder.RenameColumn(
                name: "SourceFileContents",
                table: "Products",
                newName: "SourceFile");
        }
    }
}
