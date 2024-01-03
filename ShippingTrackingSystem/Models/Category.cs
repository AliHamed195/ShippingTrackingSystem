using Microsoft.AspNetCore.Identity;
using System.ComponentModel.DataAnnotations;

namespace ShippingTrackingSystem.Models
{
    public class Category
    {
        [Key]
        public int Id { get; set; }

        [Required(ErrorMessage = "Please enter a Category Name")]
        [Display(Name = "Category Name")]
        [StringLength(256)]
        public string Name { get; set; }

        [Required(ErrorMessage = "Please enter a Description")]
        public string Description { get; set; }

        public bool IsDeleted { get; set; } = false;

        public DateTime CreatedOn { get; set; } = DateTime.Now;

        public virtual ICollection<Product> Products { get; set; }
    }
}
