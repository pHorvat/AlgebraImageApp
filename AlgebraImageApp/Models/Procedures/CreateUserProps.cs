using MsSqlSimpleClient.Attributes.Procedures;

namespace AlgebraImageApp.Models.Procedures;

public class CreateUserProps
{
    public string username { get; set; }
    public string password { get; set; }
    public UserRole Type { get; set; } 
    public UserTier Tier { get; set; }
    
    [SqlOutput]
    public int IDUser { get; set; }
}