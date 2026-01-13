using RetailPulse.Services.Interfaces;
using RetailPulse.DTOs;
using RetailPulse.Data;
using Microsoft.EntityFrameworkCore;

namespace RetailPulse.Services
{
    public class AnalyticsService : IAnalyticsService
    {
        private readonly RetailPulseDbContext _context;
        public AnalyticsService(RetailPulseDbContext context)
        {
            _context = context;
        }

        //====================
        // Get Daily Stats
        //====================
        public async Task<DashboardResponse> GetDailyStatsAsync()
        {
            var today = DateTime.UtcNow.Date;

            var todayRevenue = await _context.Sales.Where(s => s.SoldAt.Date >= today).SumAsync(s => s.TotalPrice);
            var totalSalesToday = await _context.Sales.Where(s => s.SoldAt.Date >= today).CountAsync();

            var lowStockProducts = await _context.Products.Where(p => p.Stock < 10).Select(p => $"{p.Name} ({p.Stock} left)").ToListAsync();

            var response = new DashboardResponse
            {
                TodayRevenue = todayRevenue,
                TotalSalesToday = totalSalesToday,
                LowStockProducts =  lowStockProducts

            };

            return response;
        }

    }
}