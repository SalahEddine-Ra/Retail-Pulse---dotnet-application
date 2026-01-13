using RetailPulse.Models;
using RetailPulse.DTOs;

namespace RetailPulse.Services.Interfaces
{
    public interface IAnalyticsService
    {
        Task<DashboardResponse> GetDailyStatsAsync();
    }
}