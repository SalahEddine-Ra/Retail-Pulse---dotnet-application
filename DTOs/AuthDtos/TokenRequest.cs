using System.ComponentModel.DataAnnotations;

namespace RetailPulse.DTOs.AuthDtos
{
    public class TokenRequest
    {
        [Required]
        public string AccessToken { get; set; }
        
        [Required]
        public string RefreshToken { get; set; }
    }
}