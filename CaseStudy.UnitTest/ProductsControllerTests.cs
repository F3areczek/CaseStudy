using CaseStudy.WebApi.Controllers.v1;
using CaseStudy.WebApi.Data.Nonpersistent;
using CaseStudy.WebApi.Data.Persistent;
using Microsoft.AspNetCore.Mvc;

namespace CaseStudy.UnitTest
{
    public class ProductsControllerTests
    {
        /// <summary>
        /// TEST - Retrieves all products from the data store and count it.
        /// </summary>
        /// <returns></returns>
        [Fact]
        public async Task GetProducts_ReturnsAllProducts()
        {
            ProductsController controller = await ProductsTestData.GetProductController();
            var result = await controller.GetProducts();
            IEnumerable<Product> products = Assert.IsAssignableFrom<IEnumerable<Product>>(result.Value);
            Assert.Equal(30, products.Count());
        }


        /// <summary>
        /// TEST - Get one concrete product according to its primary key.
        /// </summary>
        /// <returns></returns>
        [Fact]
        public async Task GetProductById_ReturnsProduct_WhenFound()
        {
            ProductsController controller = await ProductsTestData.GetProductController();
            ActionResult<Product> result = await controller.GetProductById(5);
            var okResult = Assert.IsType<ActionResult<Product>>(result);
            Assert.Equal(5, result.Value?.Id);
            Assert.Equal("Bookshelf", result.Value?.Name);
        }

        /// <summary>
        /// TEST - Get one concrete product according to its primary key - Not Found.
        /// </summary>
        /// <returns></returns>
        [Fact]
        public async Task GetProductById_ReturnsNotFound_WhenMissing()
        {
            ProductsController controller = await ProductsTestData.GetProductController();
            ActionResult<Product> result = await controller.GetProductById(444);
            Assert.IsType<NotFoundResult>(result.Result);
        }

        /// <summary>
        /// TEST - Create a new Product in the data store.
        /// </summary>
        /// <returns></returns>
        [Fact]
        public async Task PostProduct_CreatesNewProduct()
        {
            ProductsController controller = await ProductsTestData.GetProductController();
            ProductDtoCreate dto = new ProductDtoCreate
            {
                Name = "Small Table",
                Price = 1050,
                StockQuantity = 7,
                ProductImageUri = "http://cz.com/tableSmall.png"
            };

            ActionResult<Product> result = await controller.PostProduct(dto);
            var createdResult = Assert.IsType<CreatedAtActionResult>(result.Result);
            Product product = Assert.IsType<Product>(createdResult.Value);
            Assert.Equal("Small Table", product.Name);
            Assert.Equal(7, product.StockQuantity);
        }

        /// <summary>
        /// TEST - Increase stock quantity of a product.
        /// </summary>
        /// <returns></returns>
        [Fact]
        public async Task PutProductStockQuantity_IncreasesQuantity()
        {
            ProductsController controller = await ProductsTestData.GetProductController();
            ActionResult<Product> result = await controller.PutProductStockQuantity(14, 4);
            var okResult = Assert.IsType<OkObjectResult>(result.Result);
            Product product = Assert.IsType<Product>(okResult.Value);
            Assert.Equal(10, product.StockQuantity);
        }

        /// <summary>
        /// TEST - Decrease stock quantity of a product.
        /// </summary>
        /// <returns></returns>
        [Fact]
        public async Task PutProductStockQuantity_DecreasesQuantity()
        {
            ProductsController controller = await ProductsTestData.GetProductController();
            ActionResult<Product> result = await controller.PutProductStockQuantity(25, -8);
            var okResult = Assert.IsType<OkObjectResult>(result.Result);
            Product product = Assert.IsType<Product>(okResult.Value);
            Assert.Equal(3, product.StockQuantity);
        }

        /// <summary>
        /// TEST - Try to decrease stock quantity of a product below zero.
        /// </summary>
        /// <returns></returns>
        [Fact]
        public async Task PutProductStockQuantity_ReturnsBadRequest_WhenInsufficientStock()
        {
            ProductsController controller = await ProductsTestData.GetProductController();
            ActionResult<Product> result = await controller.PutProductStockQuantity(5, -15);
            var badRequest = Assert.IsType<BadRequestObjectResult>(result.Result);
            Assert.Equal("Insufficient stock quantity.", badRequest.Value);
        }

        /// <summary>
        /// TEST - Try to update stock quantity of a missing product.
        /// </summary>
        /// <returns></returns>
        [Fact]
        public async Task PutProductStockQuantity_ReturnsNotFound_WhenProductMissing()
        {
            ProductsController controller = await ProductsTestData.GetProductController();
            ActionResult<Product> result = await controller.PutProductStockQuantity(999, 5);
            Assert.IsType<NotFoundResult>(result.Result);
        }
    }
}