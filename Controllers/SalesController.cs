using RetailPulse.Services; 
using RetailPulse.Services.Interfaces; 
using RetailPulse.Models;
using Microsoft.AspNetCore.Mvc;
using RetailPulse.DTOs;
using Microsoft.AspNetCore.Authorization;
using System.Security.Claims;

namespace RetailPulse.Controllers
{
    [Authorize]
    [ApiController]
    [Route("api/[controller]")]
    public class SalesController : ControllerBase
    {
        // ===========================
        // Dependencies & Constructor
        // ===========================

        private readonly ISalesService _salesService;

        public SalesController(ISalesService salesService)
        {
            _salesService = salesService;
        }

        // ===========================
        // Process Sale Endpoint (POST)
        // ===========================

        [HttpPost]
        public async Task<IActionResult> ProcessSale([FromBody] SellProductRequest request)
        {
            try
            {
                // Retrieve user ID from claims
                string userIdString = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
                int? userId = null;
                if (!string.IsNullOrEmpty(userIdString))
                {
                    userId = int.Parse(userIdString);
                }

                // Process the sale using the sales service
                var sale = await _salesService.ProcessSaleAsync(request.ProductId, request.Quantity, userId);

                // Prepare response DTO
                var response = new SaleResponse
                {
                    Id = sale.Id,
                    ProductName = sale.Product.Name,
                    Category = sale.Product.Category,
                    UnitPrice = sale.Product.Price,
                    Quantity = sale.Quantity,
                    TotalPrice = sale.TotalPrice,
                    SoldAt = sale.SoldAt
                };

                return Ok(response);
            }
            catch (Exception ex)
            {
                // Return error response if processing fails
                return BadRequest(ex.Message);
            }
        }

        // ===========================
        // Get Sales History Endpoint (GET)
        // ===========================

        [HttpGet]
        public async Task<IActionResult> GetSalesHistory()
        {
            // Fetch sales history from the service
            var sales = await _salesService.GetSalesHistoryAsync();

            // Map sales to response DTOs
            var response = sales.Select(s => new SaleResponse
            {
                Id = s.Id,
                ProductName = s.Product.Name,
                Category = s.Product.Category,
                UnitPrice = s.Product.Price,
                Quantity = s.Quantity,
                TotalPrice = s.TotalPrice,
                SoldAt = s.SoldAt
            });
            return Ok(response);
        }
    }
}