using MsSqlSimpleClient.Attributes.Procedures;

namespace AlgebraImageApp.Models.Procedures;

public class AddPhotoProps
{
    /*
CREATE TABLE photos (
id INT IDENTITY(1,1) PRIMARY KEY,
author_id INT,
description NVARCHAR(MAX),
upload_datetime DATETIME,
image_format NVARCHAR(255),
hashtags NVARCHAR(255),
image_url NVARCHAR(255),
FOREIGN KEY (author_id) REFERENCES users(id)
    );*/
    [SqlParameterName("p_author_id")]
    public int AuthorId { get; set; }
    
    [SqlParameterName("p_description")]
    public string Description { get; set; }
    
    [SqlParameterName("p_upload_datetime")]
    public DateTime UploadTime { get; set; } 
    
    [SqlParameterName("p_image_format")]
    public string Format { get; set; }
    
    [SqlParameterName("p_hashtags")]
    public string Hashtags { get; set; }

    [SqlParameterName("p_image_url")] 
    public string Url { get; set; } = string.Empty;
    
    [SqlOutput]
    [SqlParameterName("id")]
    public int IdPhoto { get; set; }
}