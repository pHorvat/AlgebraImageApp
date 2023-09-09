using AlgebraImageApp.Aspect;
using AlgebraImageApp.Models;
using AlgebraImageApp.Models.Commands;
using AlgebraImageApp.Patterns;
using AlgebraImageApp.Patterns.Facade;
using AlgebraImageApp.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Prometheus.Client;

namespace AlgebraImageApp.Controllers;

[Route("api/[controller]")]
[ApiController]
[Authorize]

public class PhotoController : ControllerBase
{
    private IPhotoRetrievalService _photoRetrievalService;
    private IPhotoModificationService _photoModificationService;
    private IUserService _userService;
    public ICounter uploadedPhotos = Metrics.DefaultFactory.CreateCounter("PhotoUpload", "Number of uploaded photos");
    public ICounter deletedPhotos = Metrics.DefaultFactory.CreateCounter("PhotoDelete", "Number of deleted photos");

    [ExceptionHandlingAspect]
    public PhotoController(IPhotoRetrievalService photoRetrievalService,IPhotoModificationService photoModificationService, IUserService userService)
    {
        this._photoRetrievalService = photoRetrievalService;
        this._photoModificationService = photoModificationService;
        _userService = userService;

        //divider(10,0);
    }

    public void divider(int a, int b)
    {
        Console.WriteLine(a/b);
    }

    [HttpGet]
    [AllowAnonymous]
    public async Task<IActionResult> GetAllPhotosAsync()
    {
        IEnumerable<Photos> photos = await this._photoRetrievalService.GetAllPhotos();
        CustomLogger.Instance.Log("This is a log message.");
        return this.Ok(photos);
    }
    
    [HttpGet("{id}")]
    [AllowAnonymous]
    public async Task<IActionResult> GetAllPhotosAsync(int id)
    {
        Photos? photo = await this._photoRetrievalService.GetPhotoAsync(id);

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
        IEnumerable<Photos> photos = await this._photoRetrievalService.GetAllPhotosBySearch(searchTerm);

        return this.Ok(photos);
    }
    
    [HttpGet("user/{id}")]
    [AllowAnonymous]
    public async Task<IActionResult> GetAllPhotosOfUserAsync(int id)
    {
        IEnumerable<Photos> photos = await this._photoRetrievalService.GetAllPhotosOfUser(id);

        if (photos is null)
        {
            return this.NotFound();
        }

        return this.Ok(photos);
    }
    
    [HttpPost]
    [AllowAnonymous]
    //[Authorize]
    public async Task<IActionResult> CreatePhotoAsync(AddPhotoCommand command)
    {
        uploadedPhotos.Inc();
        User? user = await this._userService.GetUsernameAsync(command.authorUsername);
        UploadCheck check = new UploadCheck();
        
        if (this.ModelState.IsValid == false)
        {
            return this.BadRequest(this.ModelState);
        }

        if (user != null && check.CanUpload(user, command.Description, command.Hashtags))
        {
            int id = await this._photoModificationService.AddPhotoAsync(command);
            await _userService.UpdateConsumptionAsync(true, user.Id);
            CustomLogger.Instance.Log(command.authorUsername + " has uploaded a new photo");
            return this.Ok(id);
        }

        return this.Forbid();

    }
    
    [HttpDelete("{id}")]
    [AllowAnonymous]
    //[Authorize]
    public async Task<IActionResult> DeletePhotoAsync(int id)
    {
        deletedPhotos.Inc();
        Photos? photo = await this._photoRetrievalService.GetPhotoAsync(id);
        if (photo != null)
        {
            int userId = photo.AuthorId;
            await this._photoModificationService.DeletePhoto(id);
            await _userService.UpdateConsumptionAsync(false, userId);
            CustomLogger.Instance.Log("Photo with the id "+ id + " has been deleted");

            return this.Ok();
        }
        
        return this.BadRequest();
        
    }
    
    [HttpPut]
    [AllowAnonymous]
    //[Authorize]
    public async Task<IActionResult> UpdatePhotoAsync(UpdatePhotoCommand command)
    {
        if (this.ModelState.IsValid == false)
        {
            return this.BadRequest(this.ModelState);
        }

        try
        {
            await this._photoModificationService.UpdatePhotoAsync(command);
            CustomLogger.Instance.Log("Photo with the id "+ command.Id + " has been updated");

            return this.Ok();
        }
        catch (ArgumentNullException ex)
        {
            return this.BadRequest(ex.Message);
        }
    }
    
    
    
}