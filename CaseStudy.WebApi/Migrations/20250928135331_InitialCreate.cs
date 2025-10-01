using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace CaseStudy.WebApi.Migrations
{
    /// <inheritdoc />
    public partial class InitialCreate : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "PRODUCT",
                columns: table => new
                {
                    ID = table.Column<int>(type: "INTEGER", nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    NAME = table.Column<string>(type: "TEXT", nullable: false),
                    DESCRIPTION = table.Column<string>(type: "TEXT", nullable: true),
                    PRODUCT_IMAGE_URI = table.Column<string>(type: "TEXT", nullable: false),
                    PRICE = table.Column<decimal>(type: "TEXT", nullable: true),
                    STOCK_QUANTITY = table.Column<int>(type: "INTEGER", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_PRODUCT", x => x.ID);
                });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "PRODUCT");
        }
    }
}
