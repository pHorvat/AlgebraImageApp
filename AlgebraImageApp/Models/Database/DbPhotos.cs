using MsSqlSimpleClient.Attributes.SqlTable;

namespace AlgebraImageApp.Models.Database;

public class DbPhotos
{
    /*
    CREATE TABLE photos (
    id INT IDENTITY(1,1) PRIMARY KEY,
    author_id INT,
    description NVARCHAR(MAX),
    upload_datetime DATETIME,
    image_format NVARCHAR(255),
    image_url NVARCHAR(255),
    FOREIGN KEY (author_id) REFERENCES users(id)
        );*/
    [SqlIdentity]
    [SqlColumnName("id")]
    public int Id { get; set; }
    
    [SqlColumnName("author_id")]
    public int AuthorId { get; set; }
    
    [SqlColumnName("description")]
    public string Description { get; set; }
    
    [SqlColumnName("upload_datetime")]
    public DateTime Upload { get; set; } = DateTime.UnixEpoch;
    
    [SqlColumnName("image_format")]
    public string Format { get; set; }
    
    [SqlColumnName("image_url")]
    public string Url { get; set; }
    
    [SqlColumnName("hashtags")]
    public string Hashtags { get; set; }
    
    
}