using CaseStudy.WebApi.Data;
using CaseStudy.WebApi.Data.Interface;
using CaseStudy.WebApi.Data.Persistent;

namespace CaseStudy.WebApi.Services
{
    /// <summary>
    /// Background service that processes product stock updates from a queue.
    /// </summary>
    public class ProductStockUpdateWorker : BackgroundService
    {
        private readonly IProductStockUpdateQueue _queue;
        private readonly IServiceScopeFactory _scopeFactory;
        private readonly ILogger<ProductStockUpdateWorker> _logger;

        /// <summary>
        /// Constructor with dependency injection.
        /// </summary>
        /// <param name="queue">StockUpdate implementation of <see cref="IProductStockUpdateQueue"/></param>
        /// <param name="scopeFactory">Scope Factory</param>
        /// <param name="logger">Service for logging</param>
        public ProductStockUpdateWorker(IProductStockUpdateQueue queue, IServiceScopeFactory scopeFactory, ILogger<ProductStockUpdateWorker> logger)
        {
            _queue = queue;
            _scopeFactory = scopeFactory;
            _logger = logger;
        }

        /// <summary>
        /// Worker execute async action
        /// </summary>
        /// <param name="stoppingToken">Cacellation token to canel async operation</param>
        /// <returns></returns>
        protected override async Task ExecuteAsync(CancellationToken stoppingToken)
        {
            await foreach (var (productId, quantityChange) in _queue.DequeueAsync(stoppingToken))
            {
                try
                {
                    using (IServiceScope scope = _scopeFactory.CreateScope())
                    {
                        using (AppDbContext dbContext = scope.ServiceProvider.GetRequiredService<AppDbContext>())
                        {
                            Product? product = await dbContext.Products.FindAsync(productId);
                            if (product == null)
                            {
                                _logger.LogWarning($"Product with Id {productId} not found.");
                                continue;
                            }

                            if (product.StockQuantity + quantityChange >= 0)
                            {
                                product.StockQuantity += quantityChange;
                                await dbContext.SaveChangesAsync(stoppingToken);
                            }else
                            {
                                _logger.LogWarning($"Insufficient stock for Product {productId}. Current stock: {product.StockQuantity}, attempted change: {quantityChange}");
                                continue;
                            }
                        }
                    }

                } catch (Exception ex)
                {
                    _logger.LogError(ex, "Error processing stock update for Product {ProductId}", productId);
                }
            }
        }
    }

}
