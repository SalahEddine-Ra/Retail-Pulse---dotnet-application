namespace RetailPulse.DTOs
{
    public class SaleResponse
    {
        public int Id { get; set; }
        public string ProductName {get; set;}
        public string Category {get; set;}
        public decimal UnitPrice {get; set;}
        public int Quantity { get; set; }
        public decimal TotalPrice { get; set; }
        public DateTime SoldAt { get; set; }
    }
}