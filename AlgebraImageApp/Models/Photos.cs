
namespace AlgebraImageApp.Models;

public class Photos
{
    /*CREATE TABLE photos (
    id INT IDENTITY(1,1) PRIMARY KEY,
    author_id INT,
    description NVARCHAR(MAX),
    upload_datetime DATETIME,
    image_format NVARCHAR(255),
    image_url NVARCHAR(255),
    FOREIGN KEY (author_id) REFERENCES users(id)
);*/
    
    public int Id { get; set; } 
    public int AuthorId { get; set; }
    public string Description { get; set; } = string.Empty;
    public string authorUsername { get; set; } = string.Empty;
    public DateTime Upload { get; set; } = DateTime.UnixEpoch;
    public string Format { get; set; } = string.Empty;
    public string Url { get; set; } = string.Empty;
    public string Hashtags { get; set; } = string.Empty;





}