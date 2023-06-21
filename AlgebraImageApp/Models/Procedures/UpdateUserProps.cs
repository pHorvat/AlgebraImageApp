using MsSqlSimpleClient.Attributes.Procedures;

namespace AlgebraImageApp.Models.Procedures;

public class UpdateUserProps
{
    [SqlParameterName("id")]
    public int IdUser { get; set; }
    
    [SqlParameterName("username")]
    public string Username { get; set; } 
    
    [SqlParameterName("type")]
    public string Type { get; set; } 
    
    [SqlParameterName("package")]
    public string Tier { get; set; }


}