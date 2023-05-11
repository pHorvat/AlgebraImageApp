using MsSqlSimpleClient.Attributes.SqlTable;

namespace AlgebraImageApp.Models.Database;

public class DbUser
{
    /* public int Id { get; set; }
    public string username { get; set; } = string.Empty;
    public string password { get; set; } = string.Empty;
    public UserRole Type { get; set; } 
    public UserTier Tier { get; set; } 
    public int consumption { get; set; }
    public DateTime lastPackageChange { get; set; }*/
    
    [SqlIdentity]
    [SqlColumnName("id")]
    public int Id { get; set; }
    [SqlColumnName("username")]
    public string Username { get; set; }
    [SqlColumnName("password")]
    public string Password { get; set; }
    
    [SqlColumnName("type")]
    public string Type { get; set; } 
    
    [SqlColumnName("package")]
    public string Tier { get; set; }
    
    [SqlColumnName("current_consumption")]
    
    public int Consumption { get; set; } = 0;
    
    [SqlColumnName("last_package_change")]
    public DateTime LastPackageChange { get; set; } = DateTime.UnixEpoch;
}