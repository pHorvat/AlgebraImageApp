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
    public string Username { get; set; } = string.Empty;
    public string Password { get; set; } = string.Empty;
    public string Type { get; set; } = "Registered";
    public string Tier { get; set; } = "Free";
    public int Consumption { get; set; }
    public DateTime LastPackageChange { get; set; }

}