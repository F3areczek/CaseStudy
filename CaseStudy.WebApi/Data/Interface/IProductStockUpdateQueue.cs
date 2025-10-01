using CaseStudy.WebApi.Data.Persistent;

namespace CaseStudy.WebApi.Data.Interface
{
    /// <summary>
    /// Interface used for stop update in queue. Could be implemented in InMemory queue, Kafka, RabbitMQ, etc.
    /// </summary>
    public interface IProductStockUpdateQueue
    {
        /// <summary>
        /// 
        /// </summary>
        /// <param name="productId">Primary key of updating <see cref="Product"/></param>
        /// <param name="quantityChange">Value of quantity of <see cref="Product"/> to update</param>
        /// <returns></returns>
        public ValueTask QueueStockUpdateAsync(int productId, int quantityChange);

        /// <summary>
        /// using IAsyncEnumerable to process stock update one by one
        /// </summary>
        /// <param name="cancellationToken">Cacellation token to canel async operation</param>
        /// <returns></returns>
        public IAsyncEnumerable<(int productId, int quantityChange)> DequeueAsync(CancellationToken cancellationToken);
    }
}
