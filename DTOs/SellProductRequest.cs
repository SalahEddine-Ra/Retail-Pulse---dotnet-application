using System.ComponentModel.DataAnnotations;

namespace RetailPulse.DTOs
{
    public class SellProductRequest
    {
        [Required]
        public int ProductId { get; set; }

        [Required]
        [Range(1, 100)] 
        public int Quantity { get; set; }
    }
}