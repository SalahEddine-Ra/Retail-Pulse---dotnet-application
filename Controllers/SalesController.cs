using RetailPulse.Services; 
using RetailPulse.Services.Interfaces; 
using RetailPulse.Models;
using Microsoft.AspNetCore.Mvc;
using RetailPulse.DTOs;


namespace RetailPulse.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class SalesController : ControllerBase
    {
        private readonly ISalesService _salesService;
        public SalesController(ISalesService salesService)
        {
            _salesService = salesService;
        }
        [HttpPost]
       [HttpPost]
    public async Task<IActionResult> ProcessSale([FromBody] SellProductRequest request)
    {
        try
        {
            var sale = await _salesService.ProcessSaleAsync(request.ProductId, request.Quantity);
            
            // Map Entity -> DTO (So we don't send the circular references back)
            var response = new SaleResponse
            {
                Id = sale.Id,
                ProductName = sale.Product.Name, // Now we can include this!
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
            return BadRequest(ex.Message);
        }
    }

        [HttpGet]
        public async Task<IActionResult> GetSalesHistory()
        {
            var sales = await _salesService.GetSalesHistoryAsync();

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