using System.ComponentModel.DataAnnotations;

namespace AlgebraImageApp.Models.Commands;

public class UpdateUserCommand
{
    
    [Required]
    public int id { get; set; }
    
    
    [Required]
    [MaxLength(50)]
    public string username { get; set; }
    [MinLength(6)]
    public string password { get; set; }

    public UserRole Type { get; set; }

    public UserTier Tier { get; set; }

    public int consumption { get; set; }
    
    public DateTime lastPackageChange { get; set; }
}