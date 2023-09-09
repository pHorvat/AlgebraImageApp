using AlgebraImageApp.Models;
using AlgebraImageApp.Models.Commands;

namespace AlgebraImageApp.Services;

//OLD:
/*
public interface IPhotosService
{

Task<IEnumerable<Photos>> GetAllPhotos();
Task<IEnumerable<Photos>> GetAllPhotosOfUser(int id);
Task<IEnumerable<Photos>> GetAllPhotosBySearch(string term);
Task<Photos?> GetPhotoAsync(int id);
Task<int> AddPhotoAsync(AddPhotoCommand command);

Task UpdatePhotoAsync(UpdatePhotoCommand command);
Task DeletePhoto(int id);
}*/

    //SOLID: The refactored `PhotoService` now adheres to the Interface Segregation Principle
    //allowing clients to depend only on the methods they need.
    
    public interface IPhotoRetrievalService
    {
        Task<IEnumerable<Photos>> GetAllPhotos();
        Task<IEnumerable<Photos>> GetAllPhotosOfUser(int id);
        Task<IEnumerable<Photos>> GetAllPhotosBySearch(string term);
        Task<Photos?> GetPhotoAsync(int id);
    }

    public interface IPhotoModificationService
    {
        Task<int> AddPhotoAsync(AddPhotoCommand command);
        Task UpdatePhotoAsync(UpdatePhotoCommand command);
        Task DeletePhoto(int id);
    }
