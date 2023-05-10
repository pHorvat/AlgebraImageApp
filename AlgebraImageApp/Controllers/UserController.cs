using AlgebraImageApp.Models;
using AlgebraImageApp.Models.Commands;
using AlgebraImageApp.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace AlgebraImageApp.Controllers
{

[Route("api/[controller]")]
[ApiController]
[Authorize]
public class UserController : ControllerBase
{
     private IUserService _userService;

     public UserController(IUserService userService)
     {
          this._userService = userService;
     }

     [HttpGet]
     [AllowAnonymous]
     public async Task<IActionResult> GetAllUsersAsync()
     {
          IEnumerable<User> user = await this._userService.GetAllAsync();
          return this.Ok(user);
     }
     
     
     [HttpGet("{id}")]
     [AllowAnonymous]
     public async Task<IActionResult> GetAllUsersAsync(int id)
     {
          User? user = await this._userService.GetAsync(id);

          if (user is null)
          {
               return this.NotFound();
          }

          return this.Ok(user);
     }
     
     [HttpPost]
     public async Task<IActionResult> CreateUserAsync(CreateUserCommand command)
     {
          if (this.ModelState.IsValid == false)
          {
               return this.BadRequest(this.ModelState);
          }

          int id = await this._userService.CreateAsync(command);
          return this.Ok(id);
     }
     
     [HttpPut]
     public async Task<IActionResult> UpdateUserAsync(UpdateUserCommand command)
     {
          if (this.ModelState.IsValid == false)
          {
               return this.BadRequest(this.ModelState);
          }

          try
          {
               await this._userService.UpdateAsync(command);
               return this.Ok();
          }
          catch (ArgumentNullException ex)
          {
               return this.BadRequest(ex.Message);
          }
     }
     
     [HttpDelete("{id}")]
     public async Task<IActionResult> DeleteUserAsync(int id)
     {
          await this._userService.DeleteAsync(id);
          return this.Ok();
     }
     

}
}