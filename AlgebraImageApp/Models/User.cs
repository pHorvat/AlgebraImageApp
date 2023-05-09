namespace AlgebraImageApp.Models;

public class User
{
    /*id INT PRIMARY KEY,
    username NVARCHAR(255),
    password NVARCHAR(255),
    type VARCHAR(20) CHECK (type IN ('registered', 'anonymous', 'administrator')),
    package VARCHAR(20) CHECK (package IN ('FREE', 'PRO', 'GOLD')),
    current_consumption INT DEFAULT 0,
    last_package_change DATE,
    UNIQUE (username)*/
    
    public int Id { get; set; }
    public string username { get; set; } = string.Empty;
    public string password { get; set; } = string.Empty;
    public UserRole Type { get; set; } 
    public UserTier Tier { get; set; } 
    public int consumption { get; set; }
    public DateTime lastPackageChange { get; set; }

}