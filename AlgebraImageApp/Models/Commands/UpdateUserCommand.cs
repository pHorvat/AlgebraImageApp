using System.ComponentModel.DataAnnotations;

namespace AlgebraImageApp.Models.Commands;

public class UpdateUserCommand
{
    
    [Required]
    public int Id { get; set; }
    
    
    [Required]
    [MaxLength(50)]
    public string Username { get; set; }
    
    public string Type { get; set; }

    public string Tier { get; set; }

}