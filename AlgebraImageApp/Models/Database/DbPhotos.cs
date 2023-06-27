using MsSqlSimpleClient.Attributes.SqlTable;

namespace AlgebraImageApp.Models.Database;

public class DbPhotos
{

    [SqlIdentity]
    [SqlColumnName("id")]
    public int Id { get; set; }
    
    [SqlColumnName("author_id")]
    public int AuthorId { get; set; }
    
    [SqlColumnName("description")]
    public string Description { get; set; }
    
    [SqlColumnName("authorUsername")]
    public string authorUsername { get; set; }
    
    [SqlColumnName("upload_datetime")]
    public DateTime Upload { get; set; } = DateTime.UnixEpoch;
    
    [SqlColumnName("image_format")]
    public string Format { get; set; }
    
    [SqlColumnName("image_url")]
    public string Url { get; set; }
    
    [SqlColumnName("hashtags")]
    public string Hashtags { get; set; }
    
    
}