using AlgebraImageApp.Models;
using AlgebraImageApp.Models.Commands;
using AlgebraImageApp.Patterns;
using AlgebraImageApp.Patterns.Facade;
using AlgebraImageApp.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace AlgebraImageApp.Controllers;

[Route("api/[controller]")]
[ApiController]
[Authorize]
public class PhotoController : ControllerBase
{
    private IPhotosService _photosService;
    private IUserService _userService;
    
    public PhotoController(IPhotosService photosService, IUserService userService)
    {
        this._photosService = photosService;
        _userService = userService;

    }

    [HttpGet]
    [AllowAnonymous]
    public async Task<IActionResult> GetAllPhotosAsync()
    {
        IEnumerable<Photos> photos = await this._photosService.GetAllPhotos();
        CustomLogger.Instance.Log("This is a log message.");
        return this.Ok(photos);
    }
    
    [HttpGet("{id}")]
    [AllowAnonymous]
    public async Task<IActionResult> GetAllPhotosAsync(int id)
    {
        Photos? photo = await this._photosService.GetPhotoAsync(id);

        if (photo is null)
        {
            return this.NotFound();
        }

        return this.Ok(photo);
    }
    
    
    [HttpGet("search/{searchTerm}")]
    [AllowAnonymous]
    public async Task<IActionResult> GetAllPhotosForSearchAsync(string searchTerm)
    {
        IEnumerable<Photos> photos = await this._photosService.GetAllPhotosBySearch(searchTerm);

        return this.Ok(photos);
    }
    
    [HttpGet("user/{id}")]
    [AllowAnonymous]
    public async Task<IActionResult> GetAllPhotosOfUserAsync(int id)
    {
        IEnumerable<Photos> photos = await this._photosService.GetAllPhotosOfUser(id);

        if (photos is null)
        {
            return this.NotFound();
        }

        return this.Ok(photos);
    }
    
    [HttpPost]
    [Authorize]
    public async Task<IActionResult> CreatePhotoAsync(AddPhotoCommand command)
    {
        User? user = await this._userService.GetUsernameAsync(command.authorUsername);
        UploadCheck check = new UploadCheck();
        
        if (this.ModelState.IsValid == false)
        {
            return this.BadRequest(this.ModelState);
        }

        if (user != null && check.CanUpload(user, command.Description, command.Hashtags))
        {
            int id = await this._photosService.AddPhotoAsync(command);
            await _userService.UpdateConsumptionAsync(true, user.Id);
            CustomLogger.Instance.Log(command.authorUsername + " has uploaded a new photo");
            return this.Ok(id);
        }

        return this.Forbid();

    }
    
    [HttpDelete("{id}")]
    [Authorize]
    public async Task<IActionResult> DeletePhotoAsync(int id)
    {
        Photos? photo = await this._photosService.GetPhotoAsync(id);
        if (photo != null)
        {
            int userId = photo.AuthorId;
            await this._photosService.DeletePhoto(id);
            await _userService.UpdateConsumptionAsync(false, userId);
            CustomLogger.Instance.Log("Photo with the id "+ id + " has been deleted");

            return this.Ok();
        }
        
        return this.BadRequest();
        
    }
    
    [HttpPut]
    [Authorize]
    public async Task<IActionResult> UpdatePhotoAsync(UpdatePhotoCommand command)
    {
        if (this.ModelState.IsValid == false)
        {
            return this.BadRequest(this.ModelState);
        }

        try
        {
            await this._photosService.UpdatePhotoAsync(command);
            CustomLogger.Instance.Log("Photo with the id "+ command.Id + " has been updated");

            return this.Ok();
        }
        catch (ArgumentNullException ex)
        {
            return this.BadRequest(ex.Message);
        }
    }
    
    
    
}