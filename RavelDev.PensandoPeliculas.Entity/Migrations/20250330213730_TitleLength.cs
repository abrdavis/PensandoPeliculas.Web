using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace RavelDev.PensandoPeliculas.Entity.Migrations
{
    /// <inheritdoc />
    public partial class TitleLength : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "LengthMinutes",
                table: "Titles",
                type: "int",
                nullable: false,
                defaultValue: 0);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "LengthMinutes",
                table: "Titles");
        }
    }
}
