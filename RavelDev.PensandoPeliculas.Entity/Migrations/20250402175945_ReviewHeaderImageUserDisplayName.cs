using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace RavelDev.PensandoPeliculas.Entity.Migrations
{
    /// <inheritdoc />
    public partial class ReviewHeaderImageUserDisplayName : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "SiteDisplayName",
                table: "Users",
                type: "varchar(200)",
                nullable: false)
                .Annotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.AlterColumn<Guid>(
                name: "ReviewAuthor",
                table: "Reviews",
                type: "varchar(36)",
                nullable: false,
                collation: "ascii_general_ci",
                oldClrType: typeof(string),
                oldType: "longtext")
                .OldAnnotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.AddColumn<string>(
                name: "HeaderImageUrl",
                table: "Reviews",
                type: "varchar(2083)",
                nullable: false)
                .Annotation("MySql:CharSet", "utf8mb4");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "SiteDisplayName",
                table: "Users");

            migrationBuilder.DropColumn(
                name: "HeaderImageUrl",
                table: "Reviews");

            migrationBuilder.AlterColumn<string>(
                name: "ReviewAuthor",
                table: "Reviews",
                type: "longtext",
                nullable: false,
                oldClrType: typeof(Guid),
                oldType: "varchar(36)")
                .Annotation("MySql:CharSet", "utf8mb4")
                .OldAnnotation("Relational:Collation", "ascii_general_ci");
        }
    }
}
