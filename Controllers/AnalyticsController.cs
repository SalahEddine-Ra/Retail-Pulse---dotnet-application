using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using RetailPulse.Constants;
using RetailPulse.Services.Interfaces;
using System;

namespace RetailPulse.Controllers
{
    [Authorize]
    [ApiController]
    [Route("api/[controller]")]
    public class AnalyticsController : ControllerBase
    {
        private readonly IAnalyticsService _service;
        public AnalyticsController(IAnalyticsService service)
        {
            _service = service;
        }

        //=====================
        //Get Daily Stats
        //=====================
        [HttpGet("daily-report")]
        [Authorize(Roles = AppRole.Admin)]
        public async Task<IActionResult> GetDailyStats()
        {
            try
            {
                var stats = await _service.GetDailyStatsAsync();
                return Ok(stats);
            }
            catch (Exception ex)
            {
                
                return BadRequest($"Error generating report: {ex.Message}");
            }
        }
    }
}