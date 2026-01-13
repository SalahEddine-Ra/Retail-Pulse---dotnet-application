using System.ComponentModel.DataAnnotations;

namespace RetailPulse.DTOs
{
    public class CreateProductRequest
    {
        // Notice: NO "Id" here. The user cannot choose the ID.
        
        [Required]
        public string Name { get; set; }

        [Range(0.01, 10000)] // Validation logic lives here now!
        public decimal Price { get; set; }

        [Range(0, 1000)]
        public int Stock { get; set; }

        [Required]
        public string Category { get; set; }
    }
}