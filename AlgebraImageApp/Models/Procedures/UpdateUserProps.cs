namespace AlgebraImageApp.Models.Procedures;

public class UpdateUserProps
{
    public int IdUser { get; set; }
    public string Username { get; set; } = string.Empty;
    public string Password { get; set; } = string.Empty;
    public string Type { get; set; } = "Anonymous";
    public string Tier { get; set; } = "Free";


}