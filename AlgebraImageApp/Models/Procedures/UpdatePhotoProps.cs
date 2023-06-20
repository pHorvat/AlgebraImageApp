using MsSqlSimpleClient.Attributes.Procedures;

namespace AlgebraImageApp.Models.Procedures;

public class UpdatePhotoProps
{
    [SqlParameterName("id")]
    public int id { get; set; }
    
    [SqlParameterName("description")]
    public string Description { get; set; }
    
    [SqlParameterName("hashtags")]
    public string Hashtags { get; set; }
}