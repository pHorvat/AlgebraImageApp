using System.Data;
using AlgebraImageApp.Models;
using AlgebraImageApp.Models.Database;
using AlgebraImageApp.Models.Procedures;
using MsSqlSimpleClient.Converters;
using MsSqlSimpleClient.SqlClient.Procedures;

namespace AlgebraImageApp.Repositories;

public class SqlUserRepository : IUserRepository
{
    private readonly ISqlProcedureClient _procedureClient;

    public SqlUserRepository(ISqlProcedureClient procedureClient)
    {
        this._procedureClient = procedureClient;
    }
    
    public async Task<int> CreateAsync(string username, string password, string type, string tier)
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

    public async Task DeleteAsync(int id)
    {
        await this._procedureClient.ExecuteNonQueryAsyncWith("DeleteUser", id);

    }

    public async Task<IEnumerable<DbUser>> GetAllAsync()
    {
        DataSet data = await this._procedureClient.ExecuteQueryAsync("GetUsers");
        return data.ConvertTo<DbUser>();
    }
    
            public async Task UpdateAsync(UpdateUserProps props)
        {
            await this._procedureClient.ExecuteNonQueryAsync("UpdateUser", props);
        }
     
     
    
}