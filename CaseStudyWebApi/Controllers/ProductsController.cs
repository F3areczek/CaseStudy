using CaseStudyWebApi.Data;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace CaseStudyWebApi.Controllers
{
    /// <summary>
    /// Controller for Product endpoints
    /// </summary>
    [ApiController]
    [Route("api/[controller]")]
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
        [EndpointDescription("Call to retrieve all products from the data store.")]
        public async Task<ActionResult<IEnumerable<Product>>> GetProducts()
        {
            return await _dbContext.Products.ToListAsync();
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
        public async Task<ActionResult<Product>> PutProductStockQuantity(int id, int quantityChange)
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
    }
}
