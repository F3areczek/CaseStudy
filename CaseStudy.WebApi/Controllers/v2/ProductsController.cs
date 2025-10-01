using Asp.Versioning;
using CaseStudy.WebApi.Data;
using CaseStudy.WebApi.Data.Interface;
using CaseStudy.WebApi.Data.Nonpersistent;
using CaseStudy.WebApi.Data.Persistent;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Collections;
using System.ComponentModel.DataAnnotations;

namespace CaseStudy.WebApi.Controllers.v2
{
    /// <summary>
    /// Controller for Product endpoints
    /// </summary>
    [ApiController]
    [ApiVersion("2.0")]
    [Route("api/v{version:apiVersion}/[controller]")]
    public class ProductsController : ControllerBase
    {

        private readonly AppDbContext _dbContext;


        public ProductsController(AppDbContext dbContext)
        {
            _dbContext = dbContext;
        }

        /// <summary>
        /// Retrieves all products from the data store.
        /// </summary>
        /// <remarks>GET: api/Products.</remarks>
        [HttpGet]
        [EndpointSummary("Get all products")]
        [EndpointDescription("Call to retrieve paginated products from the data store. Support pagination.")]
        public async Task<IEnumerable<Product>> GetProducts([FromQuery] int page = 1, [FromQuery] int pageSize = 10)
        {
            // Get total count of items
            int countOfItems = await _dbContext.Products.CountAsync();

            // Retrieve paginated items
            IList<Product> products = await _dbContext.Products.Skip((page - 1) * pageSize).Take(pageSize).ToListAsync();

            // Add pagination metadata to response headers
            Response?.Headers?.Append("X-Pagination", System.Text.Json.JsonSerializer.Serialize(new PaginationMetadata(countOfItems, pageSize, page)));

            return products;
        }

        /// <summary>
        /// Get one concrete product according to its primary key.
        /// </summary>
        /// <param name="id">Primary key of <see cref="Product"/></param>
        /// <remarks>GET: api/Products/11</remarks>
        [HttpGet("{id}")]
        [EndpointSummary("Get the one product")]
        [EndpointDescription("Call to get one concrete product according to its primary key.")]
        public async Task<ActionResult<Product>> GetProductById(int id)
        {
            Product? foundProduct = await _dbContext.Products.FindAsync(id);

            if (foundProduct == null)
            {
                return NotFound();
            }

            return foundProduct;
        }

        /// <summary>
        /// Creates a new product in the data store.
        /// </summary>
        /// <param name="product">DTO product to insert</param>
        /// <remarks>POST: api/Products</remarks>
        [HttpPost]
        [EndpointSummary("Creating a new product")]
        [EndpointDescription("Call to create a new product type record in the data store.")]
        public async Task<ActionResult<Product>> PostProduct(ProductDtoCreate product)
        {
            Product productItem = new Product
            {
                Name = product.Name,
                Description = product.Description,
                ProductImageUri = product.ProductImageUri,
                Price = product.Price,
                StockQuantity = product.StockQuantity
            };

            _dbContext.Products.Add(productItem);

            await _dbContext.SaveChangesAsync();

            return CreatedAtAction(nameof(GetProductById), new { id = productItem.Id }, productItem);
        }

        /// <summary>
        /// Updates the stock quantity of a specific product.
        /// </summary>
        /// <param name="id">Primary key of updating <see cref="Product"/></param>
        /// <param name="quantityChange"> Value of quantity of <see cref="Product"/> to update</param>
        /// <remarks>PUT: api/Products/11</remarks>
        [HttpPut("{id}")]
        [EndpointSummary("Adjusting the quantity in stock")]
        [EndpointDescription("Call to update the quantity of one particular product in stock.")]
        public async Task<ActionResult<Product>> PutProductStockQuantity(int id, [Required] int quantityChange)
        {

            Product? productItem = await _dbContext.Products.FindAsync(id);

            if (productItem == null)
                return NotFound();

            if (productItem.StockQuantity + quantityChange < 0)
                return BadRequest("Insufficient stock quantity.");


            productItem.StockQuantity += quantityChange;

            await _dbContext.SaveChangesAsync();

            return Ok(productItem);
        }

        /// <summary>
        /// Adding a request to the queue for processing stock quantity.
        /// </summary>
        /// <param name="id">Primary key of updating <see cref="Product"/></param>
        /// <param name="quantityChange"> Value of quantity of <see cref="Product"/> to update</param>
        /// <param name="queue">Queue from service to process</param>
        /// <remarks>PUT: api/Products/QueueStockUpdate/11</remarks>
        [HttpPut("QueueStockUpdate/{id}")]
        [EndpointSummary("Adjusting the quantity in stock from queue")]
        [EndpointDescription("Call to add a request for stock quantity adjustment to the queue.")]
        public async Task<IActionResult> PutProductStockQuantity(int id, [Required] int quantityChange, [FromServices] IProductStockUpdateQueue queue)
        {
            await queue.QueueStockUpdateAsync(id, quantityChange);
            return Accepted(new { Message = "Stock update request accepted for processing", ProductId = id });
        }
    }
}
