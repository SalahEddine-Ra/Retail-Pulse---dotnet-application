using RetailPulse.Services.Interfaces;
using RetailPulse.DTOs.AuthDtos;
using RetailPulse.Models;
using RetailPulse.Data;
using Microsoft.EntityFrameworkCore;


namespace RetailPulse.Services
{
    public class AuthService : IAuthService
    {
        private readonly RetailPulseDbContext _context;
        public AuthService(RetailPulseDbContext context)
        {
            _context = context;
        }


        // register logic
        public async Task<User?> RegisterAsync(RegisterRequest request)
        {
            bool isExisted = await _context. Users.AnyAsync( u => u.Email == request.Email);
            if (isExisted) 
            {
               return null; 
            }

            string hashedPassword = BCrypt.Net.BCrypt.HashPassword(request.Password);

            var user = new User
            {
                Username = request.Username,
                Email = request.Email,
                PasswordHash = hashedPassword,
                Role = "User"
            };

            return user;
        }

        //login logic
        public async Task<string> LoginAsync(LoginRequest request)
        {
            var user = await _context.Users.FirstOrDefaultAsync(u => u.Email == request.Email);

            if (user == null)
            {
                return null;
            }

            bool isPasswordValid = BCrypt.Net.BCrypt.Verify(request.password, user.PasswordHash);

            if (!isPasswordValid)
            {
                return null;
            }

            return "fake-jwt-token";
        }
    }
}