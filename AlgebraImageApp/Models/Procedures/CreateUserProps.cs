using MsSqlSimpleClient.Attributes.Procedures;
using MsSqlSimpleClient.Attributes.SqlTable;

namespace AlgebraImageApp.Models.Procedures;

public class CreateUserProps
{
    [SqlParameterName("p_username")]
    public string Username { get; set; }
    
    [SqlParameterName("p_password")]
    public string Password { get; set; }
    
    [SqlParameterName("p_type")]
    public string Type { get; set; } 
    
    [SqlParameterName("p_package")]
    public string Tier { get; set; }

    [SqlParameterName("p_current_consumption")] 
    public int Consumption { get; set; } = 0;
    
    [SqlParameterName("p_last_package_change")] 
    public DateTime LastPackageChange { get; set; } = DateTime.UnixEpoch;
    
    [SqlOutput]
    [SqlParameterName("id")]
    public int IdUser { get; set; }
}