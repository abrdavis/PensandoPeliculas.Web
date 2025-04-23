using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace RavelDev.PensandoPeliculas.Entity.Migrations
{
    /// <inheritdoc />
    public partial class TitleImageDefaultValues : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<string>(
    name: "PosterThumbnailUrl",
    table: "Titles",
    type: "VARCHAR(2083)",
    nullable: false,
    defaultValue: "");

            migrationBuilder.AlterColumn<string>(
                name: "PosterUrl",
                table: "Titles",
                type: "VARCHAR(2083)",
                nullable: false,
                defaultValue: "");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {

        }
    }
}
