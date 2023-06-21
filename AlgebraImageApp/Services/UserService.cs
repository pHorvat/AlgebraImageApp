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
        return (await this._repository.GetAllUsersAsync()).Select(dbUser => new User
        {
            Id = dbUser.Id,
            Username = dbUser.Username,
            Password = dbUser.Password,
            Type = dbUser.Type,
            Tier = dbUser.Tier,
            Consumption = dbUser.Consumption,
            LastPackageChange = dbUser.LastPackageChange
        });
    }

    public async Task<User?> GetAsync(int id)
    {
        return (await this._repository.GetAllUsersAsync())
            .Where(dbUser => dbUser.Id == id)
            .Select(dbUser => new User
            {
                Id = dbUser.Id,
                Username = dbUser.Username,
                Password = dbUser.Password,
                Type = dbUser.Type,
                Tier = dbUser.Tier,
                Consumption = dbUser.Consumption,
                LastPackageChange = dbUser.LastPackageChange
            })
            .SingleOrDefault();
    }
    
    public async Task<User?> GetUsernameAsync(string username)
    {
        return (await this._repository.GetAllUsersAsync())
            .Where(dbUser => dbUser.Username == username)
            .Select(dbUser => new User
            {
                Id = dbUser.Id,
                Username = dbUser.Username,
                Password = dbUser.Password,
                Type = dbUser.Type,
                Tier = dbUser.Tier,
                Consumption = dbUser.Consumption,
                LastPackageChange = dbUser.LastPackageChange
            })
            .SingleOrDefault();
    }

    
    
    public async Task<int> CreateAsync(CreateUserCommand command)
    {
        string username = command.Username;
        string password = command.Password;
        string type = command.Type;
        string tier = command.Tier;

        int id = await this._repository.CreateUserAsync(username, password, type, tier);
        return id; 
    }

    public async Task UpdateAsync(UpdateUserCommand command)
    {
       
        UpdateUserProps props = new UpdateUserProps
        {
            IdUser = command.Id
        };

        User? current = await this.GetAsync(command.Id);

        if (current is null)
        {
            throw new ArgumentNullException($"No User found for id: {command.Id}");
        }

        props.Username = command.Username ?? current.Username;
        props.Tier = command.Tier;
        props.Type = command.Type;

        await this._repository.UpdateUserAsync(props);
    
    }

    public async Task DeleteAsync(int id)
    {
        await this._repository.DeleteUserAsync(id);
    }
    
    public async Task<int> GetConsumption(int id)
    {
        int consumption = await this._repository.GetUserConsumptionAsync(id);
        return consumption;
    }
}