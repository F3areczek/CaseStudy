using CaseStudy.WebApi.Attributes;
using System.ComponentModel.DataAnnotations;

namespace CaseStudy.WebApi.Data.Nonpersistent
{
    /// <summary>
    /// Data Transfere Object for POSTing new product into database
    /// </summary>
    public class ProductDtoCreate
    {
        /// <summary>
        /// The name of the available product.
        /// </summary>
        [Required]
        public required string Name { get; set; }

        /// <summary>
        /// Description of available product.
        /// </summary>
        public string? Description { get; set; }

        /// <summary>
        /// Link to main product image
        /// </summary>
        [Required]
        [ImageUri]
        public required string ProductImageUri { get; set; }

        /// <summary>
        /// Price of one piece of product
        /// </summary>
        public decimal? Price { get; set; }

        /// <summary>
        /// Number of available items of the product in stock.
        /// </summary>
        public int? StockQuantity { get; set; }
    }
}
