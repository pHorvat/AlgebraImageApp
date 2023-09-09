using System.Data;
using AlgebraImageApp.Aspect;
using AlgebraImageApp.Models.Database;
using AlgebraImageApp.Models.Procedures;
using MsSqlSimpleClient.Converters;
using MsSqlSimpleClient.SqlClient.Direct;
using MsSqlSimpleClient.SqlClient.Procedures;

namespace AlgebraImageApp.Repositories;

public class SqlUserRepository : IUserRepository
{
    private readonly ISqlProcedureClient _procedureClient;
    private readonly ISqlDirectClient  _directClient;

    public SqlUserRepository(ISqlProcedureClient procedureClient, ISqlDirectClient directClient)
    {
        this._procedureClient = procedureClient;
        this._directClient = directClient;
    }
    
    public async Task<int> CreateUserAsync(string username, string password, string type, string tier)
    {
        CreateUserProps props = new CreateUserProps
        {
            Username = username,
            Password = password,
            Type = type,
            Tier = tier
        };
        await this._procedureClient.ExecuteNonQueryAsync("CreateUser", props);
        return props.IdUser;
    }

    public async Task DeleteUserAsync(int id)
    {
        await this._procedureClient.ExecuteNonQueryAsyncWith("DeleteUser", id);

    }

    public async Task<IEnumerable<DbUser>> GetAllUsersAsync()
    {
        DataSet data = await this._procedureClient.ExecuteQueryAsync("GetUsers");
        return data.ConvertTo<DbUser>();
    }
    
    public async Task<int> GetUserConsumptionAsync(string id)
    {
        //DataSet consumption = (await _directClient.ExecuteQueryAsync("select * from photos where author_id="+id+";"));
        DataSet consumption = await this._procedureClient.ExecuteQueryAsync("GetConsumption",new { @UserId = id});
        IEnumerable<DbUser> cons = consumption.ConvertTo<DbUser>();
        Console.WriteLine(cons.Count());
        return cons.Count();

    }
    
    [LoggingAspect]
    public async Task UpdateConsumptionAsync(bool operation, int id)
    {
        if (operation)
        {
            await _directClient.ExecuteQueryAsync("UPDATE users SET current_consumption = current_consumption + 1 WHERE id = "+id+";");
        }
        else
        {
            await _directClient.ExecuteQueryAsync("UPDATE users SET current_consumption = current_consumption - 1 WHERE id = "+id+";");
        }
        
    }
    
    public async Task UpdateLastPackageChangeAsync(int id)
    {
        DateTime myDateTime = DateTime.Now;
        string sqlFormattedDate = myDateTime.ToString("s");
        await _directClient.ExecuteQueryAsync("UPDATE users SET last_package_change = GETDATE() WHERE id = "+id+";");
    }
    
    
    public async Task UpdateUserAsync(UpdateUserProps props)
    {
        await this._procedureClient.ExecuteNonQueryAsync("UpdateUser", props);
    }
     
     
    
}