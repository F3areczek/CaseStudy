namespace CaseStudy.WebApi.Data.Nonpersistent
{
    /// <summary>
    /// Response model for product stock update operations.
    /// </summary>
    public class ProductStockUpdateResponse
    {
        /// <summary>
        /// Stock update message
        /// </summary>
        public string Message { get; set; } = default!;

        /// <summary>
        /// Stock update product id
        /// </summary>
        public int ProductId { get; set; }
    }
}
