using CaseStudy.WebApi.Data.Persistent;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace CaseStudy.WebApi.Migrations
{
    /// <inheritdoc />
    public partial class InitialData : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.InsertData(
                table: "PRODUCT",
                columns: new[] { "ID", "NAME", "DESCRIPTION", "PRODUCT_IMAGE_URI", "PRICE", "STOCK_QUANTITY" },
                values: new object[,]
                {
                    { 1, "Table", null, "http://cz.com/table.png", 3200m, 11 },
                    { 2, "Chair", null, "http://cz.com/chair.png", 1250m, 5 },
                    { 3, "Lamp", null, "http://cz.com/lamp.png", 450m, 13 },
                    { 4, "Sofa", null, "http://cz.com/sofa.png", 8700m, 3 },
                    { 5, "Bookshelf", null, "http://cz.com/bookshelf.png", 2100m, 7 },
                    { 6, "Desk", null, "http://cz.com/desk.png", 3600m, 9 },
                    { 7, "Bed Frame", null, "http://cz.com/bedframe.png", 9400m, 4 },
                    { 8, "Wardrobe", null, "http://cz.com/wardrobe.png", 12500m, 2 },
                    { 9, "Coffee Table", null, "http://cz.com/coffeetable.png", 2700m, 6 },
                    { 10, "Office Chair", null, "http://cz.com/officechair.png", 2950m, 15 },
                    { 11, "Dining Table", null, "http://cz.com/diningtable.png", 11200m, 2 },
                    { 12, "Nightstand", null, "http://cz.com/nightstand.png", 1800m, 10 },
                    { 13, "TV Stand", null, "http://cz.com/tvstand.png", 4200m, 5 },
                    { 14, "Armchair", null, "http://cz.com/armchair.png", 5600m, 6 },
                    { 15, "Stool", null, "http://cz.com/stool.png", 850m, 20 },
                    { 16, "Bench", null, "http://cz.com/bench.png", 1900m, 8 },
                    { 17, "Cupboard", null, "http://cz.com/cupboard.png", 7200m, 3 },
                    { 18, "Dresser", null, "http://cz.com/dresser.png", 6600m, 4 },
                    { 19, "Shelf Unit", null, "http://cz.com/shelfunits.png", 2500m, 9 },
                    { 20, "Sideboard", null, "http://cz.com/sideboard.png", 5100m, 5 },
                    { 21, "Recliner", null, "http://cz.com/recliner.png", 8300m, 6 },
                    { 22, "Bar Stool", null, "http://cz.com/barstool.png", 1150m, 12 },
                    { 23, "Bedside Table", null, "http://cz.com/bedside.png", 2100m, 7 },
                    { 24, "Chest of Drawers", null, "http://cz.com/chest.png", 7400m, 5 },
                    { 25, "Shoe Rack", null, "http://cz.com/shoerack.png", 2300m, 11 },
                    { 26, "TV Cabinet", null, "http://cz.com/tvcabinet.png", 6100m, 3 },
                    { 27, "Corner Sofa", null, "http://cz.com/cornersofa.png", 13900m, 2 },
                    { 28, "Loveseat", null, "http://cz.com/loveseat.png", 7200m, 4 },
                    { 29, "Rocking Chair", null, "http://cz.com/rockingchair.png", 4600m, 3 },
                    { 30, "Folding Table", null, "http://cz.com/foldingtable.png", 2700m, 9 }
                });
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            for (int id = 1; id <= 30; id++)
            {
                migrationBuilder.DeleteData(
                    table: "PRODUCT",
                    keyColumn: "ID",
                    keyValue: id
                );
            }
        }
    }
}
