namespace AlgebraImageApp.Models.Procedures;

public class UpdateUserProps
{
    public int IDUser { get; set; }
    public string username { get; set; } = string.Empty;
    public string password { get; set; } = string.Empty;
    public UserRole Type { get; set; } = UserRole.Anonymous;
    public UserTier Tier { get; set; } = UserTier.FREE;


}