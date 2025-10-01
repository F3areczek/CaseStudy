using CaseStudy.WebApi.Data.Interface;
using System.Runtime.CompilerServices;
using System.Threading.Channels;

namespace CaseStudy.WebApi.Data.Nonpersistent
{
    /// <summary>
    /// Implementation of in-memory stock update queue using Channel to handle stock updates asynchronously.
    /// It supports queuing stock updates and dequeuing them for processing by implementing <see cref="IProductStockUpdateQueue"/>.
    /// </summary>
    public class InMemoryStockUpdateQueue : IProductStockUpdateQueue
    {
        private readonly Channel<(int productId, int quantityChange)> _queue;

        public InMemoryStockUpdateQueue()
        {
            _queue = Channel.CreateUnbounded<(int, int)>();
        }

        /// <summary>
        /// Add a stock update request to the queue.
        /// </summary>
        /// <param name="productId">Primary key of product to update</param>
        /// <param name="quantityChange">Quantity of stock to update for product</param>
        /// <returns></returns>
        public ValueTask QueueStockUpdateAsync(int productId, int quantityChange)
            => _queue.Writer.WriteAsync((productId, quantityChange));

        /// <summary>
        /// Retrieves stock update requests from the queue.
        /// </summary>
        /// <param name="cancellationToken">Cacellation token to canel async operation</param>
        /// <returns></returns>
        public async IAsyncEnumerable<(int productId, int quantityChange)> DequeueAsync([EnumeratorCancellation] CancellationToken cancellationToken)
        {
            while (await _queue.Reader.WaitToReadAsync(cancellationToken))
            {
                if (cancellationToken.IsCancellationRequested) yield break;
                while (_queue.Reader.TryRead(out var item))
                {
                    if (cancellationToken.IsCancellationRequested) yield break;
                    yield return item;
                }
                    
            }
        }
    }
}
