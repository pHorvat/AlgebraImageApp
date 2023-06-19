using AlgebraImageApp.Models;
using AlgebraImageApp.Models.Commands;

namespace AlgebraImageApp.Services;

public interface IPhotosService
{
    Task<IEnumerable<Photos>> GetAllPhotos();
    Task<IEnumerable<Photos>> GetAllPhotosOfUser(int id);
    Task<IEnumerable<Photos>> GetAllPhotosBySearch(string term);
    Task<Photos?> GetPhotoAsync(int id);
    Task<int> AddPhotoAsync(AddPhotoCommand command);
    //Task UpdateAsync(UpdateUserCommand command);
    Task DeletePhoto(int id);
}