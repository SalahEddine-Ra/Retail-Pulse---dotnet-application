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

        public async Task<Sale> ProcessSaleAsync(int productId, int quantity)
        {
            var productExisted = await _context.Products.FindAsync(productId);
            if (productExisted == null)
            {
                throw new Exception("Product not found!");
            }

            if (productExisted.Stock < quantity)
            {
                throw new Exception("Not enough stock!");
            }

            productExisted.Stock = productExisted.Stock - quantity;

            var sale = new Sale
            {
                ProductId = productId,
                Quantity = quantity,
                TotalPrice = productExisted.Price * quantity,
                SoldAt = DateTime.UtcNow
            };

            _context.Sales.Add(sale);
            await _context.SaveChangesAsync();
            return sale;

        }
        public async Task<List<Sale>> GetSalesHistoryAsync()
        {
            return await _context.Sales
                .Include(s => s.Product)
                .OrderByDescending(s => s.SoldAt)
                .ToListAsync();
        }
    }
}

