using System;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace RavelDev.PensandoPeliculas.Entity.Migrations
{
    /// <inheritdoc />
    public partial class ReviewSchema : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Titles",
                columns: table => new
                {
                    TitleId = table.Column<int>(type: "int", nullable: false)
                        .Annotation("MySql:ValueGenerationStrategy", MySqlValueGenerationStrategy.IdentityColumn),
                    TitleName = table.Column<string>(type:"TEXT", nullable: false),
                    ReleaseDate = table.Column<DateTime>(type: "DATETIME", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Titles", x => x.TitleId);
                });

            migrationBuilder.CreateTable(
                name: "Reviews",
                columns: table => new
                {
                    ReviewId = table.Column<int>(type: "int", nullable: false)
                        .Annotation("MySql:ValueGenerationStrategy", MySqlValueGenerationStrategy.IdentityColumn),
                    ReviewDate = table.Column<DateTime>(type: "DATETIME", nullable: false),
                    DateUpdated = table.Column<DateTime>(type: "DATETIME", nullable: false),
                    TitleId = table.Column<int>(type: "int", nullable: false),
                    ReviewTitle = table.Column<string>(type: "TEXT", nullable: false),
                    ReviewText = table.Column<string>(type: "TEXT", nullable: false),
                    ReviewScore = table.Column<decimal>(type: "decimal(18,2)", nullable: false),
                    IsVisible = table.Column<bool>(type: "bit", nullable: false),
                    ReviewAuthor = table.Column<string>(type: "nvarchar(512)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Reviews", x => x.ReviewId);
                    table.ForeignKey(
                        name: "FK_Reviews_Titles_TitleId",
                        column: x => x.TitleId,
                        principalTable: "Titles",
                        principalColumn: "TitleId",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_Reviews_TitleId",
                table: "Reviews",
                column: "TitleId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Reviews");

            migrationBuilder.DropTable(
                name: "Titles");
        }
    }
}
