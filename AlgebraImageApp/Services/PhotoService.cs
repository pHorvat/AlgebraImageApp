using AlgebraImageApp.Models;
using AlgebraImageApp.Models.Commands;
using AlgebraImageApp.Repositories;

namespace AlgebraImageApp.Services;

public class PhotoService : IPhotosService
{

    private readonly IPhotosRepository _repository;

    public PhotoService(IPhotosRepository repository)
    {
        this._repository = repository;
    }

    public async Task<IEnumerable<Photos>> GetAllPhotos()
    {
        return (await this._repository.GetAllPhotosAsync()).Select(dbPhoto => new Photos()
        {
            Id = dbPhoto.Id,
            AuthorId = dbPhoto.AuthorId,
            Description = dbPhoto.Description,
            Format = dbPhoto.Format,
            Hashtags = dbPhoto.Hashtags,
            Upload = dbPhoto.Upload,
            Url = dbPhoto.Url
        });
    }

    public async Task<Photos?> GetPhotoAsync(int id)
    {
        return (await this._repository.GetAllPhotosAsync())
            .Where(dbPhoto => dbPhoto.Id == id)
            .Select(dbPhoto => new Photos()
            {
                Id = dbPhoto.Id,
                AuthorId = dbPhoto.AuthorId,
                Description = dbPhoto.Description,
                Format = dbPhoto.Format,
                Hashtags = dbPhoto.Hashtags,
                Upload = dbPhoto.Upload,
                Url = dbPhoto.Url
            })
            .SingleOrDefault();
    }

    public async Task<int> AddPhotoAsync(AddPhotoCommand command)
    {
        int authorId = command.AuthorId;
        string description = command.Description;
        string format = command.Format;
        string hashtags = command.Hashtags;
        string url = command.Url;
        DateTime upload = command.Upload;

        int id = await this._repository.AddPhotoAsync(authorId,description,format,hashtags,url);
        return id; 
    }

    public async Task DeletePhoto(int id)
    {
        await this._repository.DeletePhotoAsync(id);
    }
}