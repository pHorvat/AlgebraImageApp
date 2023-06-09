using System.Data;
using AlgebraImageApp.Models.Database;
using AlgebraImageApp.Models.Procedures;
using MsSqlSimpleClient.Converters;
using MsSqlSimpleClient.SqlClient.Procedures;

namespace AlgebraImageApp.Repositories;

public class SqlPhotosRepository : IPhotosRepository
{
    private readonly ISqlProcedureClient _procedureClient;

    public SqlPhotosRepository(ISqlProcedureClient procedureClient)
    {
        this._procedureClient = procedureClient;
    }
    
    
    public async Task<int> AddPhotoAsync(int authorid, string desciption, string format, string hashtags, string url)
    {
        AddPhotoProps props = new AddPhotoProps
        {
            AuthorId = authorid,
            Description = desciption,
            Format = format,
            Hashtags = hashtags,
            Url = url
        };
        await this._procedureClient.ExecuteNonQueryAsync("AddPhoto", props);
        return props.IdPhoto;
    }

    public async Task DeletePhotoAsync(int id)
    {
        await this._procedureClient.ExecuteNonQueryAsyncWith("DeletePhoto", id);
    }

    public async Task<IEnumerable<DbPhotos>> GetAllPhotosAsync()
    {
        DataSet data = await this._procedureClient.ExecuteQueryAsync("GetPhotos");
        return data.ConvertTo<DbPhotos>();    
    }
    
    
    public async Task<IEnumerable<DbPhotos>> GetAllPhotosOfUserAsync(int id)
    {
        DataSet data = await this._procedureClient.ExecuteQueryAsync("GetPhotosOfUser", new { @authorId = id});
        return data.ConvertTo<DbPhotos>();    
    }
}