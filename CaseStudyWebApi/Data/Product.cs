namespace CaseStudyWebApi.Data
{
    /// <summary>
    /// An object representing a product available in the e-shop.
    /// </summary>
    public class Product
    {
        /// <summary>
        /// Primary key for the product.
        /// </summary>
        public int Id { get; set; }

        /// <summary>
        /// The name of the available product.
        /// </summary>
        public required string Name { get; set; }

        /// <summary>
        /// Description of available product.
        /// </summary>
        public string? Description { get; set; }

        /// <summary>
        /// Link to main product image
        /// </summary>
        public required Uri ProductImageUri { get; set; }

        /// <summary>
        /// Price of one piece of product
        /// </summary>
        public decimal? Price { get; set; }

        /// <summary>
        /// Number of available items of the product in stock.
        /// </summary>
        public int? Stock { get; set; }
    }
}
