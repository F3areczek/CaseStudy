using CaseStudyWebApi.Attributes;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace CaseStudyWebApi.Data
{
    /// <summary>
    /// An object representing a product available in the e-shop.
    /// </summary>
    [Table("PRODUCT")]
    public class Product
    {
        /// <summary>
        /// Primary key for the product.
        /// </summary>
        [Key]
        [Column("ID")]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }

        /// <summary>
        /// The name of the available product.
        /// </summary>
        [Required]
        [Column("NAME")]
        public required string Name { get; set; }

        /// <summary>
        /// Description of available product.
        /// </summary>
        [Column("DESCRIPTION")]
        public string? Description { get; set; }

        /// <summary>
        /// Link to main product image
        /// </summary>
        [Required]
        [Column("PRODUCT_IMAGE_URI")]
        [ImageUri]
        public required string ProductImageUri { get; set; }

        /// <summary>
        /// Price of one piece of product
        /// </summary>
        [Column("PRICE")]
        public decimal? Price { get; set; }

        /// <summary>
        /// Number of available items of the product in stock.
        /// </summary>
        [Column("STOCK_QUANTITY")]
        public int? StockQuantity { get; set; }
    }
}
