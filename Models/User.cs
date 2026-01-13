using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace RetailPulse.Models
{
    public class User
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id {get; set;}
        [Required]        
        [MaxLength(50)]
        public string Username {get; set;}

        [Required]
        [EmailAddress]
        public string Email {get; set;}

        [Required]
        public string PasswordHash {get; set;}

        public string Role {get; set;}
    }
}