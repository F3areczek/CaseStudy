using CaseStudy.WebApi.Controllers.v1;
using CaseStudy.WebApi.Data;
using CaseStudy.WebApi.Data.Persistent;
using Microsoft.EntityFrameworkCore;

namespace CaseStudy.UnitTest
{
    /// <summary>
    /// Provides test data by seeding them into in-memory store.
    /// </summary>
    public class ProductsTestData
    {

        /// <summary>
        /// Creates and returns an instance of <see cref="ProductsController"/> initialized with an in-memory database.
        /// </summary>
        /// <remarks>This method sets up an in-memory database, seeds it with product data, and then
        /// creates a new  <see cref="ProductsController"/> instance using the seeded database.
        public static async Task<ProductsController> GetProductController()
        {
            AppDbContext inMemoryDatabase = GetInMemoryDb();
            await SeedProducts(inMemoryDatabase);
            return new ProductsController(inMemoryDatabase);
        }

        private static AppDbContext GetInMemoryDb()
        {
            DbContextOptions<AppDbContext> options = new DbContextOptionsBuilder<AppDbContext>()
                .UseInMemoryDatabase(databaseName: "TestDb_" + Guid.NewGuid())
                .Options;

            return new AppDbContext(options);
        }

        private static async Task SeedProducts(AppDbContext db)
        {
            db.Products.AddRange(
                new Product { Id = 1, Name = "Table", StockQuantity = 11, Price = 3200, ProductImageUri = "http://cz.com/table.png" },
                new Product { Id = 2, Name = "Chair", StockQuantity = 5, Price = 1250, ProductImageUri = "http://cz.com/chair.png" },
                new Product { Id = 3, Name = "Lamp", StockQuantity = 13, Price = 450, ProductImageUri = "http://cz.com/lamp.png" },
                new Product { Id = 4, Name = "Sofa", StockQuantity = 3, Price = 8700, ProductImageUri = "http://cz.com/sofa.png" },
                new Product { Id = 5, Name = "Bookshelf", StockQuantity = 7, Price = 2100, ProductImageUri = "http://cz.com/bookshelf.png" },
                new Product { Id = 6, Name = "Desk", StockQuantity = 9, Price = 3600, ProductImageUri = "http://cz.com/desk.png" },
                new Product { Id = 7, Name = "Bed Frame", StockQuantity = 4, Price = 9400, ProductImageUri = "http://cz.com/bedframe.png" },
                new Product { Id = 8, Name = "Wardrobe", StockQuantity = 2, Price = 12500, ProductImageUri = "http://cz.com/wardrobe.png" },
                new Product { Id = 9, Name = "Coffee Table", StockQuantity = 6, Price = 2700, ProductImageUri = "http://cz.com/coffeetable.png" },
                new Product { Id = 10, Name = "Office Chair", StockQuantity = 15, Price = 2950, ProductImageUri = "http://cz.com/officechair.png" },
                new Product { Id = 11, Name = "Dining Table", StockQuantity = 2, Price = 11200, ProductImageUri = "http://cz.com/diningtable.png" },
                new Product { Id = 12, Name = "Nightstand", StockQuantity = 10, Price = 1800, ProductImageUri = "http://cz.com/nightstand.png" },
                new Product { Id = 13, Name = "TV Stand", StockQuantity = 5, Price = 4200, ProductImageUri = "http://cz.com/tvstand.png" },
                new Product { Id = 14, Name = "Armchair", StockQuantity = 6, Price = 5600, ProductImageUri = "http://cz.com/armchair.png" },
                new Product { Id = 15, Name = "Stool", StockQuantity = 20, Price = 850, ProductImageUri = "http://cz.com/stool.png" },
                new Product { Id = 16, Name = "Bench", StockQuantity = 8, Price = 1900, ProductImageUri = "http://cz.com/bench.png" },
                new Product { Id = 17, Name = "Cupboard", StockQuantity = 3, Price = 7200, ProductImageUri = "http://cz.com/cupboard.png" },
                new Product { Id = 18, Name = "Dresser", StockQuantity = 4, Price = 6600, ProductImageUri = "http://cz.com/dresser.png" },
                new Product { Id = 19, Name = "Shelf Unit", StockQuantity = 9, Price = 2500, ProductImageUri = "http://cz.com/shelfunits.png" },
                new Product { Id = 20, Name = "Sideboard", StockQuantity = 5, Price = 5100, ProductImageUri = "http://cz.com/sideboard.png" },
                new Product { Id = 21, Name = "Recliner", StockQuantity = 6, Price = 8300, ProductImageUri = "http://cz.com/recliner.png" },
                new Product { Id = 22, Name = "Bar Stool", StockQuantity = 12, Price = 1150, ProductImageUri = "http://cz.com/barstool.png" },
                new Product { Id = 23, Name = "Bedside Table", StockQuantity = 7, Price = 2100, ProductImageUri = "http://cz.com/bedside.png" },
                new Product { Id = 24, Name = "Chest of Drawers", StockQuantity = 5, Price = 7400, ProductImageUri = "http://cz.com/chest.png" },
                new Product { Id = 25, Name = "Shoe Rack", StockQuantity = 11, Price = 2300, ProductImageUri = "http://cz.com/shoerack.png" },
                new Product { Id = 26, Name = "TV Cabinet", StockQuantity = 3, Price = 6100, ProductImageUri = "http://cz.com/tvcabinet.png" },
                new Product { Id = 27, Name = "Corner Sofa", StockQuantity = 2, Price = 13900, ProductImageUri = "http://cz.com/cornersofa.png" },
                new Product { Id = 28, Name = "Loveseat", StockQuantity = 4, Price = 7200, ProductImageUri = "http://cz.com/loveseat.png" },
                new Product { Id = 29, Name = "Rocking Chair", StockQuantity = 3, Price = 4600, ProductImageUri = "http://cz.com/rockingchair.png" },
                new Product { Id = 30, Name = "Folding Table", StockQuantity = 9, Price = 2700, ProductImageUri = "http://cz.com/foldingtable.png" }
            );
            await db.SaveChangesAsync();
        }
    }
}
