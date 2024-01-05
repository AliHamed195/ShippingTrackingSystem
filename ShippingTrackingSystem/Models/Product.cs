using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace ShippingTrackingSystem.Models
{
    public class Product
    {
        [Key]
        public int Id { get; set; }

        [Required(ErrorMessage = "Product name is required.")]
        [Display(Name = "Product Name")]
        [StringLength(256)]
        public string Name { get; set; }

        [Required(ErrorMessage = "Description name is required.")]
        public string Description { get; set; }

        [Required(ErrorMessage = "Product price is required.")]
        public int Price { get; set; }

        [Required(ErrorMessage = "Stock quantity is required.")]
        [Display(Name = "Stock Quantity")]
        public int StockQuantity { get; set; }

        [Display(Name = "Product Image")]
        public string? ImagePath { get; set; }

        [NotMapped]
        public IFormFile? ImageFile { get; set; }

        public DateTime CreatedOn { get; set; } = DateTime.Now;

        public bool IsDeleted { get; set; } = false;

        public int CategoryId { get; set; }
        public Category? Category { get; set; }
    }
}
