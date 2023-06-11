using System.ComponentModel.DataAnnotations;

namespace AlgebraImageApp.Models.Commands;

public class AddPhotoCommand
{
    [Required] 
    public int AuthorId { get; set; }
    [MaxLength(300)]
    public string Description { get; set; }
    
    [Required]
    public string authorUsername { get; set; }
    
    [Required] 
    public DateTime Upload { get; set; } = DateTime.UnixEpoch;
    
    [Required] 
    public string Format { get; set; }
    
    [Required] 
    public string Url { get; set; }
    
    [MaxLength(200)]
    public string Hashtags { get; set; }
}