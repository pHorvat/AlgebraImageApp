using AlgebraImageApp.Models.Database;

namespace AlgebraImageApp.Repositories;

public interface IPhotosRepository
{
    /*
CREATE TABLE photos (
id INT IDENTITY(1,1) PRIMARY KEY,
author_id INT,
description NVARCHAR(MAX),
upload_datetime DATETIME,
image_format NVARCHAR(255),
hashtags NVARCHAR(255),
image_url NVARCHAR(255),
FOREIGN KEY (author_id) REFERENCES users(id)
    );*/
    
    
    Task<int> AddPhotoAsync(int authorid, string desciption, string format, string hashtags, string url, string authorusername);

    Task DeletePhotoAsync(int id);
    public Task<IEnumerable<DbPhotos>> GetAllPhotosAsync();
    public Task<IEnumerable<DbPhotos>> GetAllPhotosBySearchAsync(string searchTerm);
    public Task<IEnumerable<DbPhotos>> GetAllPhotosOfUserAsync(int id);
    //Task UpdateAsync(UpdateUserProps props);

}