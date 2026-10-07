using System.ComponentModel.DataAnnotations;

namespace Product_Catalog_Application.Models
{
    public class Product
    {
        public int Id { get; set; }

        [Required(ErrorMessage = "Product name is required")]
        [StringLength(60, MinimumLength = 2, ErrorMessage = "Name must be between 2 and 60 characters")]
        public string Name { get; set; } = string.Empty;

        [Required(ErrorMessage = "Category is required")]
        public string Category { get; set; } = string.Empty;

        [Required(ErrorMessage = "Price is required")]
        [Range(0.01, 100000.00, ErrorMessage = "Price must be greater than 0")]
        [DataType(DataType.Currency)]
        public decimal Price { get; set; }

        [Display(Name = "Stock Quantity")]
        [Range(0, 10000, ErrorMessage = "Stock cannot be negative")]
        public int StockQuantity { get; set; }

        public string Description { get; set; } = string.Empty;
    }
}