using RetailPulse.Models;
// Notice: We don't need 'using RetailPulse.DTOs' anymore!

namespace RetailPulse.Services
{
    public interface ISalesService
    {
        // Pure inputs: Just the ID and the Quantity
// Change the line to this:
        Task<Sale> ProcessSaleAsync(int productId, int quantity, int? userId);
        Task<List<Sale>> GetSalesHistoryAsync();
    }
}