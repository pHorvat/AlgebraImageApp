using AlgebraImageApp.Models;
using AlgebraImageApp.Models.Commands;

namespace AlgebraImageApp.Services;

public interface IPhotosService
{
    Task<IEnumerable<Photos>> GetAllPhotos();
    Task<Photos?> GetPhotoAsync(int id);
    Task<int> AddPhotoAsync(AddPhotoCommand command);
    //Task UpdateAsync(UpdateUserCommand command);
    Task DeletePhoto(int id);
}