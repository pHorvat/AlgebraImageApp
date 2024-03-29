﻿using System.Data;
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
    
    
    public async Task<int> AddPhotoAsync(int authorid, string desciption, string format, string hashtags, string url, string authorusername, DateTime upload)
    {
        AddPhotoProps props = new AddPhotoProps
        {
            
            authorUsername = authorusername,
            AuthorId = authorid,
            Description = desciption,
            Format = format,
            Hashtags = hashtags,
            Url = url,
            UploadTime = DateTime.Now.AddHours(5)
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
    
    public async Task<IEnumerable<DbPhotos>> GetAllPhotosBySearchAsync(string term)
    {
        DataSet data = await this._procedureClient.ExecuteQueryAsync("GetPhotosBySearch", new { @searchTerm = term});
        return data.ConvertTo<DbPhotos>();    
    }
    
    public async Task UpdatePhotoAsync(UpdatePhotoProps props)
    {
        await this._procedureClient.ExecuteNonQueryAsync("UpdatePhoto", props);
    }
    
}