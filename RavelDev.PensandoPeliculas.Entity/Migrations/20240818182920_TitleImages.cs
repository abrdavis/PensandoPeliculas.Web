using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace RavelDev.PensandoPeliculas.Entity.Migrations
{
    /// <inheritdoc />
    public partial class TitleImages : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "PosterThumbnailUrl",
                table: "Titles",
                type: "TEXT",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<string>(
                name: "PosterUrl",
                table: "Titles",
                type: "TEXT",
                nullable: false,
                defaultValue: "");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "PosterThumbnailUrl",
                table: "Titles");

            migrationBuilder.DropColumn(
                name: "PosterUrl",
                table: "Titles");
        }
    }
}
