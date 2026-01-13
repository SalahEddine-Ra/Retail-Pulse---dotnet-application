using RetailPulse.Services; 
using RetailPulse.Services.Interfaces; 
using RetailPulse.Models;
using Microsoft.AspNetCore.Mvc;
using RetailPulse.DTOs;

namespace RetailPulse.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class ProductsController : ControllerBase
    {
        private readonly IInventoryService _service;

        public ProductsController(IInventoryService service)
        {
            _service = service;
        }

        // Get All products
        [HttpGet]
        public async Task<IActionResult> GetAllProducts()
        {
            var products = await _service.GetAllProductsAsync();
            var responce = products.Select(p => new ProductResponse
            {
                Id = p.Id,
                Name = p.Name,
                Price = p.Price,
                Stock = p.Stock,
                Category = p.Category
            }).ToList();

            return Ok(responce);
        }


        // Add a product
        [HttpPost]
        public async Task<IActionResult> AddProductAsync([FromBody] CreateProductRequest request)
        {
            var newProduct = new Product
            {
                Name = request.Name,
                Price = request.Price,
                Stock = request.Stock,
                Category = request.Category
            };

            var createdProduct = await _service.AddProductAsync(newProduct);

            var response = new ProductResponse
            {
                Id = createdProduct.Id,
                Name = createdProduct.Name,
                Price = createdProduct.Price,
                Stock = createdProduct.Stock,
                Category = createdProduct.Category
            };

            return CreatedAtAction(nameof(GetAllProducts), new { id = response.Id }, response);
        }


        // Modify a product
        [HttpPut("{Id}")]
        public async Task<IActionResult> UpdateProduct(int Id, [FromBody] CreateProductRequest request)
        {
            var productToUpdate = new Product
            {
                Name = request.Name,
                Price = request.Price,
                Stock = request.Stock,
                Category = request.Category
            };

            var updatedProduct = await _service.UpdateProductAsync(Id, productToUpdate);
            
            if (updatedProduct == null)
            {
                return NotFound($"The Product with ID {Id} not found!");
            }
            
            var responce = new ProductResponse
            {
                Id = updatedProduct.Id,
                Name = updatedProduct.Name,
                Price = updatedProduct.Price,
                Stock = updatedProduct.Stock,
                Category = updatedProduct.Category
            };
            return Ok(responce);

        }


        // Delete a product
        [HttpDelete("{Id}")]
        public async Task<IActionResult> DeleteProduct(int Id)
        {
            var isDeleted = await _service.DeleteProductAsync(Id);

            if (!isDeleted)
            {
                return NotFound($"Product with ID {Id} not found!");
            }

            return NoContent();
        }
        
    }
}