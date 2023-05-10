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
    public int id { get; set; }
    public string username { get; set; }
    public string password { get; set; }
    public UserRole Type { get; set; } 
    public UserTier Tier { get; set; }
    public int consumption { get; set; } = 0;
    public DateTime lastPackageChange { get; set; } = DateTime.UnixEpoch;
}