using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Waseet.Migrations
{
    /// <inheritdoc />
    public partial class init100 : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "ID_Identity",
                table: "Students",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<string>(
                name: "ID_Identity",
                table: "Owners",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "ID_Identity",
                table: "Students");

            migrationBuilder.DropColumn(
                name: "ID_Identity",
                table: "Owners");
        }
    }
}
