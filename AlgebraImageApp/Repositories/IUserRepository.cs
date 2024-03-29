﻿using AlgebraImageApp.Models;
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

    Task<int> CreateUserAsync(string username, string password, string type, string tier);

    Task DeleteUserAsync(int id);
    public Task<IEnumerable<DbUser>> GetAllUsersAsync();
    Task UpdateUserAsync(UpdateUserProps props);

    Task<int> GetUserConsumptionAsync(string id);
    Task UpdateConsumptionAsync(bool operation, int id);
    Task UpdateLastPackageChangeAsync(int id);


}