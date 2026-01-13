using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace RetailPulse.Models
{
public class Product
{
    [Key]
    [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
    public int Id {get; set;}
    
    [Required]
    [MaxLength(100)]
    public string Name {get; set;}

    [Required]
    public decimal Price {get; set;}

    [Required]
    public int Stock {get; set;}

    [Required]
    public string Category {get; set;}

    public virtual ICollection<Sale> Sales { get; set; } = new List<Sale>();
}
}