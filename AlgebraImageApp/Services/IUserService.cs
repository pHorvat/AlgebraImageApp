using AlgebraImageApp.Models;
using AlgebraImageApp.Models.Commands;

namespace AlgebraImageApp.Services;

public interface IUserService
{
    Task<IEnumerable<User>> GetAllAsync();
    Task<User?> GetAsync(int id);
    Task<User?> GetUsernameAsync(string username);
    Task<int> CreateAsync(CreateUserCommand command);
    Task UpdateAsync(UpdateUserCommand command);
    Task DeleteAsync(int id);
}