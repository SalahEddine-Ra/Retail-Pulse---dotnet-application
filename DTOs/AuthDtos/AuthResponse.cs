namespace RetailPulse.DTOs.AuthDtos
{
    public class AuthResponse
    {
        public string Token { get; set; }        // The 15-minute Access Token
        public string RefreshToken { get; set; } // The 7-day Renewal Token
        public DateTime Expiration { get; set; } // When the Access Token dies
    }
}