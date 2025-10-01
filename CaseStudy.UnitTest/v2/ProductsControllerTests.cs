using CaseStudy.WebApi.Controllers.v2;
using CaseStudy.WebApi.Data.Interface;
using CaseStudy.WebApi.Data.Nonpersistent;
using CaseStudy.WebApi.Data.Persistent;
using Microsoft.AspNetCore.Mvc;
using Moq;

namespace CaseStudy.UnitTest.v2
{
    public class ProductsControllerTests
    {
        /// <summary>
        /// TEST - Retrieves products from the data store with default pagination (page 1, page size 10).
        /// </summary>
        /// <returns></returns>
        [Fact]
        public async Task GetProducts_ReturnWithDefaultPagination()
        {
            ProductsController controller = await ProductsTestData.GetProductControllerV2();
            var result = await controller.GetProducts();
            IEnumerable<Product> products = Assert.IsAssignableFrom<IEnumerable<Product>>(result);
            Assert.Equal(10, products.Count());
        }

        /// <summary>
        /// TEST - Retrieves products from the data store on 2nd page with page size 17.
        /// </summary>
        /// <returns></returns>
        [Fact]
        public async Task GetProducts_ReturnsPagination()
        {
            ProductsController controller = await ProductsTestData.GetProductControllerV2();
            var result = await controller.GetProducts(2,17);
            IEnumerable<Product> products = Assert.IsAssignableFrom<IEnumerable<Product>>(result);
            Assert.Equal(13, products.Count());
        }


        /// <summary>
        /// TEST - Get one concrete product according to its primary key.
        /// </summary>
        /// <returns></returns>
        [Fact]
        public async Task GetProductById_ReturnsProduct_WhenFound()
        {
            ProductsController controller = await ProductsTestData.GetProductControllerV2();
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
            ProductsController controller = await ProductsTestData.GetProductControllerV2();
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
            ProductsController controller = await ProductsTestData.GetProductControllerV2();
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
            ProductsController controller = await ProductsTestData.GetProductControllerV2();
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
            ProductsController controller = await ProductsTestData.GetProductControllerV2();
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
            ProductsController controller = await ProductsTestData.GetProductControllerV2();
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
            ProductsController controller = await ProductsTestData.GetProductControllerV2();
            ActionResult<Product> result = await controller.PutProductStockQuantity(999, 5);
            Assert.IsType<NotFoundResult>(result.Result);
        }

        /// <summary>
        /// TEST - Queues a stock update request and returns Accepted response.
        /// </summary>
        /// <returns></returns>
        [Fact]
        public async Task PutProductStockQuantity_QueuesRequest_ReturnsAccepted()
        {
            int productId = 45;
            int quantityChange = 11;

            // Mock the queue
            Mock<IProductStockUpdateQueue> mockQueue = new Mock<IProductStockUpdateQueue>();
            mockQueue
                .Setup(q => q.QueueStockUpdateAsync(productId, quantityChange))
                .Returns(ValueTask.CompletedTask)
                .Verifiable();

            ProductsController controller = new ProductsController(null!); // Empty DbContext for this test
            var result = await controller.PutProductStockQuantity(productId, quantityChange, mockQueue.Object);

            AcceptedResult acceptedResult = Assert.IsType<AcceptedResult>(result);
            ProductStockUpdateResponse stockUpdateResponse = Assert.IsAssignableFrom<ProductStockUpdateResponse>(acceptedResult.Value);

            Assert.Equal("Stock update request accepted for processing", stockUpdateResponse.Message);
            Assert.Equal(productId, stockUpdateResponse.ProductId);

            // Verify the queue was called
            mockQueue.Verify(q => q.QueueStockUpdateAsync(productId, quantityChange), Times.Once);
        }
    }
}