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

        // GET: api/Products
        [HttpGet]
        [EndpointSummary("Get all products")]
        [EndpointDescription("Call to retrieve all products from the data store.")]
        public async Task<ActionResult<IEnumerable<Product>>> GetProducts()
        {
            return await _dbContext.Products.ToListAsync();
        }

        // GET: api/Products/11
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

        // POST: api/Products
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

        // PUT: api/Products/11
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
