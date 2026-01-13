using RetailPulse.DTOs.AuthDtos;
using RetailPulse.Models;

namespace RetailPulse.Services.Interfaces
{
    public interface IAuthService
    {
        Task<User?> RegisterAsync(RegisterRequest request);
        Task<AuthResponse?> LoginAsync(LoginRequest request);
        Task<AuthResponse?> RefreshTokenAsync(string token, string refreshToken);
    }
}