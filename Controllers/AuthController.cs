using RetailPulse.Services; 
using RetailPulse.Services.Interfaces; 
using RetailPulse.Models;
using Microsoft.AspNetCore.Mvc;
using RetailPulse.DTOs.AuthDtos;


namespace RetailPulse.Controllers
{   
    [ApiController]
    [Route("api/[controller]")]
    public class AuthController : ControllerBase
    {
        private readonly IAuthService _service;
        public AuthController(IAuthService service)
        {
            _service = service;
        }

        // Register a new user
        [HttpPost("Register")]
        public async Task<IActionResult> RegisterUser([FromBody] RegisterRequest request)
        {
            var isRegisterd = await _service.RegisterAsync(request);

            if (isRegisterd == null)
            {
                return BadRequest("User already Exists");
            }

            return Ok(new {message = "User registerd successfully"});
        }
    }
}