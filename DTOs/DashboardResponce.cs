namespace RetailPulse.DTOs
{
    public class DashboardResponse
    {
        public decimal TodayRevenue { get; set; }
        public int TotalSalesToday { get; set; }
        public List<string> LowStockProducts { get; set; } = new List<string>();
    }
}