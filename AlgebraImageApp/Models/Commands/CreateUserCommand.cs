using System.ComponentModel.DataAnnotations;

namespace AlgebraImageApp.Models.Commands;

public class CreateUserCommand
{

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