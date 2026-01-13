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

        [HttpPost("Register")]
        public async Task<IActionResult> RegisterUser([FromBody] RegisterRequest request)
        {
            var user = await _service.RegisterAsync(request);

            if (user == null)
            {
                return BadRequest("User already exists");
            }

            return Ok(new { message = "User registered successfully" });
        }

        [HttpPost("Login")]
        public async Task<IActionResult> LoginUser([FromBody] LoginRequest request)
        {
            var authResponse = await _service.LoginAsync(request); 
            
            if (authResponse == null)
            {
                return Unauthorized("Invalid Credentials");
            }
            
            // Return the full object (Access Token + Refresh Token)
            return Ok(authResponse);
        }

        [HttpPost("RefreshToken")]
        public async Task<IActionResult> RefreshToken([FromBody] TokenRequest request)
        {
            if (request is null)
            {
                return BadRequest("Invalid client request");
            }

            var result = await _service.RefreshTokenAsync(request.AccessToken, request.RefreshToken);

            if (result == null)
            {
                return BadRequest("Invalid tokens");
            }

            return Ok(result);
        }
    }
}