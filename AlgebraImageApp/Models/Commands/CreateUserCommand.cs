using System.ComponentModel.DataAnnotations;

namespace AlgebraImageApp.Models.Commands;

public class CreateUserCommand
{
    /*    public int Id { get; set; }
    public string username { get; set; } = string.Empty;
    public string password { get; set; } = string.Empty;
    public UserRole Type { get; set; } 
    public UserTier Tier { get; set; } 
    public int consumption { get; set; }
    public DateTime lastPackageChange { get; set; }*/
    
    
    
    [Required]
    [MaxLength(50)]
    public string Username { get; set; } = string.Empty;
    [Required]
    [MinLength(6)]
    public string Password { get; set; } = string.Empty;

    [Required] 
    public string Type { get; set; } = "Registered";

    [Required] 
    public string Tier { get; set; } = "Free";

    [Required] 
    public int Consumption { get; set; } = 0;
    
    public DateTime LastPackageChange { get; set; } = DateTime.UnixEpoch;
    




}