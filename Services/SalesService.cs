using RetailPulse.Data;
using RetailPulse.Models;
using Microsoft.EntityFrameworkCore;
using RetailPulse.Services.Interfaces;
using RetailPulse.Controllers;

namespace RetailPulse.Services
{
    public class SalesService : ISalesService
    {
        private readonly RetailPulseDbContext _context; 
        public SalesService(RetailPulseDbContext context)
        {
            _context = context;
        }

        //===========================
        // Process a sale
        //===========================
        
        public async Task<Sale> ProcessSaleAsync(int productId, int quantity, int? userId)
        {
            var product = await _context.Products.FindAsync(productId);
            if (product == null) throw new Exception("Product not found");

            if (product.Stock < quantity) throw new Exception("Not enough stock");

            product.Stock -= quantity;

            // Create Sale Record with User ID
            var sale = new Sale
            {
                ProductId = productId,
                Quantity = quantity,
                TotalPrice = product.Price * quantity,
                SoldAt = DateTime.UtcNow,
                UserId = userId 
            };

            _context.Sales.Add(sale);
            await _context.SaveChangesAsync();
            return sale;
        }

        //===========================
        // Get Sales History
        //===========================

        public async Task<List<Sale>> GetSalesHistoryAsync()
        {
            return await _context.Sales
                .Include(s => s.Product)
                .Include(s => s.User)
                .OrderByDescending(s => s.SoldAt)
                .ToListAsync();
        }
    }
}

