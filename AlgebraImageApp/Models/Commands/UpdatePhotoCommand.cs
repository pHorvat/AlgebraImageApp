using System.ComponentModel.DataAnnotations;

namespace AlgebraImageApp.Models.Commands;

public class UpdatePhotoCommand
{
    [Required]
    public int Id { get; set; }
    
    [MaxLength(300)]
    public string Description { get; set; }
    
    [MaxLength(200)]
    public string Hashtags { get; set; }
}