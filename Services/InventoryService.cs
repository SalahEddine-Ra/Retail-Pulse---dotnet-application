using RetailPulse.Services.Interfaces;
using RetailPulse.Data;
using RetailPulse.Models;
using Microsoft.EntityFrameworkCore;

namespace RetailPulse.Services
{
    public class InventoryService : IInventoryService
    {
        // Database context dependency
        private readonly RetailPulseDbContext _context;

        // Constructor
        public InventoryService(RetailPulseDbContext context)
        {
            _context = context;
        }

        // Add a new product
        public async Task<Product> AddProductAsync(Product product)
        {
            _context.Products.Add(product);
            await  _context.SaveChangesAsync();
            return product;
        }

        //===========================
        // Retrieve all products
        //===========================

        public async Task<List<Product>> GetAllProductsAsync()
        {
            List<Product> products = await _context.Products.ToListAsync();
            return products;
        }

        //===========================
        // Update a product
        //===========================

        public async Task<Product?> UpdateProductAsync(int Id, Product product)
        {
            var productExisted = await _context.Products.FindAsync(Id);
            if (productExisted == null) return null;

            productExisted.Name = product.Name;
            productExisted.Price = product.Price;
            productExisted.Stock = product.Stock;
            productExisted.Category = product.Category;

            await _context.SaveChangesAsync();
            return productExisted;
        }

        //===========================
        // Delete a product
        //===========================
        public async Task<bool> DeleteProductAsync(int Id)
        {
            var product = await _context.Products.FindAsync(Id);
            if (product == null) return false;

            _context.Products.Remove(product);
            await _context.SaveChangesAsync();
            return true;
        }
    }
}