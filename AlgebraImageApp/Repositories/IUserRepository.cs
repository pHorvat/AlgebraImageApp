using AlgebraImageApp.Models;
using AlgebraImageApp.Models.Database;
using AlgebraImageApp.Models.Procedures;

namespace AlgebraImageApp.Repositories;

public interface IUserRepository
{
    /* public int Id { get; set; }
    public string username { get; set; } = string.Empty;
    public string password { get; set; } = string.Empty;
    public UserRole Type { get; set; } 
    public UserTier Tier { get; set; } 
    public int consumption { get; set; }
    public DateTime lastPackageChange { get; set; }*/

    Task<int> CreateAsync(string username, string password, UserRole type, UserTier tier);

    Task DeleteAsync(int id);
    public Task<IEnumerable<DbUser>> GetAllAsync();
    Task UpdateAsync(UpdateUserProps props);

    
}