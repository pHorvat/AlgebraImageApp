using AlgebraImageApp.Models;
using AlgebraImageApp.Models.Commands;
using AlgebraImageApp.Models.Procedures;
using AlgebraImageApp.Patterns;
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
            authorUsername = dbPhoto.authorUsername,
            Id = dbPhoto.Id,
            AuthorId = dbPhoto.AuthorId,
            Description = dbPhoto.Description,
            Format = dbPhoto.Format,
            Hashtags = dbPhoto.Hashtags,
            Upload = dbPhoto.Upload,
            Url = dbPhoto.Url
        });
    }
    
    public async Task<IEnumerable<Photos>> GetAllPhotosOfUser(int id)
    {
        return (await this._repository.GetAllPhotosOfUserAsync(id)).Select(dbPhoto => new Photos()
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
    
    public async Task<IEnumerable<Photos>> GetAllPhotosBySearch(string term)
    {
        return (await this._repository.GetAllPhotosBySearchAsync(term)).Select(dbPhoto => new Photos()
        {
            authorUsername = dbPhoto.authorUsername,
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
        PhotoBuilder photoBuilder = new PhotoBuilder()
            .SetAuthorId(command.AuthorId)
            .SetDescription(command.Description)
            .SetFormat(command.Format)
            .SetUrl(command.Url)
            .SetAuthorUsername(command.authorUsername)
            .SetHashtags(command.Hashtags);

        int id = await photoBuilder.AddPhotoAsync(_repository);

        return id; 
    }
    
    public async Task UpdatePhotoAsync(UpdatePhotoCommand command)
    {
       
        UpdatePhotoProps props = new UpdatePhotoProps
        {
            id = command.Id
        };

        Photos? current = await this.GetPhotoAsync(command.Id);

        if (current is null)
        {
            throw new ArgumentNullException($"No Photo found for id: {command.Id}");
        }

        props.Description = command.Description ?? current.Description;
        props.Hashtags = command.Hashtags ?? current.Hashtags;


        await this._repository.UpdatePhotoAsync(props);
    
    }

    public async Task DeletePhoto(int id)
    {
        await this._repository.DeletePhotoAsync(id);
    }
}