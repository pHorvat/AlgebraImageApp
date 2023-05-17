﻿using AlgebraImageApp.Models;
using AlgebraImageApp.Models.Commands;
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
    
    public PhotoController(IPhotosService photosService)
    {
        this._photosService = photosService;
    }
    
    [HttpGet]
    [AllowAnonymous]
    public async Task<IActionResult> GetAllPhotosAsync()
    {
        IEnumerable<Photos> photos = await this._photosService.GetAllPhotos();
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
    
    [HttpPost]
    [Authorize]
    public async Task<IActionResult> CreatePhotoAsync(AddPhotoCommand command)
    {
        if (this.ModelState.IsValid == false)
        {
            return this.BadRequest(this.ModelState);
        }

        int id = await this._photosService.AddPhotoAsync(command);
        return this.Ok(id);
    }
    
    [HttpDelete("{id}")]
    [Authorize]
    public async Task<IActionResult> DeletePhotoAsync(int id)
    {
        await this._photosService.DeletePhoto(id);
        return this.Ok();
    }
    
}