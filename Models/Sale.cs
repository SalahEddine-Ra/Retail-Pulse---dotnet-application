namespace RetailPulse.Models
{
    public class Sale
    {
        public int Id { get; set; }
        public int ProductId { get; set; }
        public Product Product { get; set; }
        public int Quantity { get; set; }
        public decimal TotalPrice { get; set; }
        public DateTime SoldAt { get; set; } = DateTime.UtcNow;

        public int? UserId { get; set; } 
        public User? User { get; set; } 
    }
}