using RetailPulse.Models;

namespace RetailPulse.Services.Interfaces
{
    public interface IInventoryService
    {
        Task<Product> AddProductAsync(Product product);
        Task<List<Product>> GetAllProductsAsync();
        Task<Product?> UpdateProductAsync(int id, Product product);
        Task<bool> DeleteProductAsync(int id);
    }
}