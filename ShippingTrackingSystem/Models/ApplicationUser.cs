using Microsoft.AspNetCore.Identity;
using System.ComponentModel.DataAnnotations;

namespace ShippingTrackingSystem.Models
{
    public class ApplicationUser : IdentityUser
    {
        [Required(ErrorMessage = "User Address is required.")]
        public string Address { get; set; }

        public DateTime CreatedOn { get; set; } = DateTime.Now;
    }
}
