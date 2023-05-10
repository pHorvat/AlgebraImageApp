using AlgebraImageApp.Models;
using AlgebraImageApp.Models.Commands;
using AlgebraImageApp.Models.Procedures;
using AlgebraImageApp.Repositories;

namespace AlgebraImageApp.Services;

public class UserService : IUserService
{
    private readonly IUserRepository _repository;

    public UserService(IUserRepository repository)
    {
        this._repository = repository;
    }
    
    /* public int Id { get; set; }
    public string username { get; set; } = string.Empty;
    public string password { get; set; } = string.Empty;
    public UserRole Type { get; set; } 
    public UserTier Tier { get; set; } 
    public int consumption { get; set; }
    public DateTime lastPackageChange { get; set; }*/
    
    public async Task<IEnumerable<User>> GetAllAsync()
    {
        return (await this._repository.GetAllAsync()).Select(dbUser => new User
        {
            Id = dbUser.id,
            username = dbUser.username,
            password = dbUser.password,
            Type = dbUser.Type,
            Tier = dbUser.Tier,
            consumption = dbUser.consumption,
            lastPackageChange = dbUser.lastPackageChange
        });
    }

    public async Task<User?> GetAsync(int id)
    {
        return (await this._repository.GetAllAsync())
            .Where(dbUser => dbUser.id == id)
            .Select(dbUser => new User
            {
                Id = dbUser.id,
                username = dbUser.username,
                password = dbUser.password,
                Type = dbUser.Type,
                Tier = dbUser.Tier,
                consumption = dbUser.consumption,
                lastPackageChange = dbUser.lastPackageChange
            })
            .SingleOrDefault();
    }

    
    
    public async Task<int> CreateAsync(CreateUserCommand command)
    {
        string username = command.username;
        string password = command.password;
        UserRole type = command.Type;
        UserTier tier = command.Tier;

        int id = await this._repository.CreateAsync(username, password, type, tier);
        return id; 
    }

    public async Task UpdateAsync(UpdateUserCommand command)
    {
       
        UpdateUserProps props = new UpdateUserProps
        {
            IDUser = command.id
        };

        User? current = await this.GetAsync(command.id);

        if (current is null)
        {
            throw new ArgumentNullException($"No User found for id: {command.id}");
        }

        props.username = command.username ?? current.username;
        props.password = command.password ?? current.password;
        props.Tier = command.Tier;
        props.Type = command.Type;

        await this._repository.UpdateAsync(props);
    
    }

    public async Task DeleteAsync(int id)
    {
        await this._repository.DeleteAsync(id);
    }
}