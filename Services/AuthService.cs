using RetailPulse.Services.Interfaces;
using RetailPulse.DTOs.AuthDtos;
using RetailPulse.Models;
using RetailPulse.Data;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using System.Security.Cryptography; 
using RetailPulse.Constants;

namespace RetailPulse.Services
{
    public class AuthService : IAuthService
    {
        private readonly RetailPulseDbContext _context;
        private readonly IConfiguration _configuration;

        public AuthService(RetailPulseDbContext context, IConfiguration configuration)
        {
            _context = context;
            _configuration = configuration;
        }

        // ==========================================
        // 1. REGISTER
        // ==========================================
        public async Task<User?> RegisterAsync(RegisterRequest request)
        {
            if (await _context.Users.AnyAsync(u => u.Email == request.Email))
            {
                return null;
            }

            var user = new User
            {
                Username = request.Username,
                Email = request.Email,
                PasswordHash = BCrypt.Net.BCrypt.HashPassword(request.Password),
                Role = AppRole.User
            };

            _context.Users.Add(user);
            await _context.SaveChangesAsync(); // FIX: Now actually saves to DB

            return user;
        }

        // ==========================================
        // 2. LOGIN
        // ==========================================
        public async Task<AuthResponse?> LoginAsync(LoginRequest request)
        {
            var user = await _context.Users.FirstOrDefaultAsync(u => u.Email == request.Email);

            // Check if user exists and password is correct
            if (user == null || !BCrypt.Net.BCrypt.Verify(request.Password, user.PasswordHash))
            {
                return null;
            }

            // Generate Tokens
            var token = CreateToken(user);
            var refreshToken = GenerateRefreshToken();

            // Save Refresh Token to DB
            user.RefreshToken = refreshToken;
            user.RefreshTokenExpiryTime = DateTime.UtcNow.AddDays(7);
            await _context.SaveChangesAsync();

            return new AuthResponse
            {
                Token = new JwtSecurityTokenHandler().WriteToken(token),
                RefreshToken = refreshToken,
                Expiration = token.ValidTo
            };
        }

        // ==========================================
        // 3. REFRESH TOKEN
        // ==========================================
        public async Task<AuthResponse?> RefreshTokenAsync(string token, string refreshToken)
        {
            // 1. Recover Identity from the EXPIRED Access Token
            var principal = GetPrincipalFromExpiredToken(token);
            if (principal == null) return null;

            // 2. Find the user
            var username = principal.Identity?.Name;
            var user = await _context.Users.FirstOrDefaultAsync(u => u.Username == username);
            var days = _configuration.GetValue<int>("Jwt:RefreshTokenExpirationDays");

            // 3. Check if Refresh Token matches and is not expired
            if (user == null || user.RefreshToken != refreshToken || user.RefreshTokenExpiryTime <= DateTime.UtcNow)
            {
                return null;
            }

            // 4. Generate NEW tokens
            var newAccessToken = CreateToken(user);
            var newRefreshToken = GenerateRefreshToken();

            // 5. Update DB
            user.RefreshToken = newRefreshToken;
            user.RefreshTokenExpiryTime = DateTime.UtcNow.AddDays(days);
            await _context.SaveChangesAsync();

            return new AuthResponse
            {
                Token = new JwtSecurityTokenHandler().WriteToken(newAccessToken),
                RefreshToken = newRefreshToken,
                Expiration = newAccessToken.ValidTo
            };
        }

        // ==========================================
        // PRIVATE HELPER METHODS
        // ==========================================

        private JwtSecurityToken CreateToken(User user)
        {
            var authClaims = new List<Claim>
            {
                new Claim(ClaimTypes.Name, user.Username),
                new Claim(ClaimTypes.NameIdentifier, user.Id.ToString()),
                new Claim(ClaimTypes.Role, user.Role)
            };

            var authSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_configuration["Jwt:Key"]));
            var minutes = _configuration.GetValue<int>("Jwt:AccessTokenExpirationMinutes");

            return new JwtSecurityToken(
                issuer: _configuration["Jwt:Issuer"],
                audience: _configuration["Jwt:Audience"],
                expires: DateTime.UtcNow.AddMinutes(minutes), // Access Token lives for 15 mins
                claims: authClaims,
                signingCredentials: new SigningCredentials(authSigningKey, SecurityAlgorithms.HmacSha256)
            );
        }

        private string GenerateRefreshToken()
        {
            var randomNumber = new byte[32];
            using (var rng = RandomNumberGenerator.Create())
            {
                rng.GetBytes(randomNumber);
                return Convert.ToBase64String(randomNumber);
            }
        }

        private ClaimsPrincipal? GetPrincipalFromExpiredToken(string? token)
        {
            var tokenValidationParameters = new TokenValidationParameters
            {
                ValidateAudience = false,
                ValidateIssuer = false,
                ValidateIssuerSigningKey = true,
                IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_configuration["Jwt:Key"])),
                ValidateLifetime = false // Critical: Allow expired tokens
            };

            var tokenHandler = new JwtSecurityTokenHandler();
            
            try 
            {
                // We use try-catch because ValidateToken throws if the format is garbage
                var principal = tokenHandler.ValidateToken(token, tokenValidationParameters, out SecurityToken securityToken);
                
                if (securityToken is not JwtSecurityToken jwtSecurityToken || 
                    !jwtSecurityToken.Header.Alg.Equals(SecurityAlgorithms.HmacSha256, StringComparison.InvariantCultureIgnoreCase))
                {
                    throw new SecurityTokenException("Invalid token");
                }

                return principal;
            }
            catch
            {
                return null;
            }
        }
    }
}