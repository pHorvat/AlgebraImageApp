using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using AlgebraImageApp.Models;
using AlgebraImageApp.Models.Commands;
using AlgebraImageApp.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;

namespace AlgebraImageApp.Controllers
{

[Route("api/[controller]")]
[ApiController]
[Authorize]
public class UserController : ControllerBase
{
     private IUserService _userService;
     private readonly string _secretKey = "ZcK#K5KdDtq8Bx%%ByhKg9BhUrtw^M6aXrnUYwQEPWn9";

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
     
     [HttpPost("register")]
     [AllowAnonymous]
     public async Task<IActionResult> RegisterAsync(CreateUserCommand command)
     {
          if (this.ModelState.IsValid == false)
          {
               return this.BadRequest(this.ModelState);
          }

          string passwordHash = BCrypt.Net.BCrypt.HashPassword(command.Password);
          command.Password = passwordHash;
          
          
          int id = await this._userService.CreateAsync(command);
          return this.Ok(id);
     }
     
     [HttpPost("login")]
     [AllowAnonymous]
     public async Task<IActionResult> LoginAsync(CreateUserCommand command)
     {
          if (this.ModelState.IsValid == false)
          {
               return this.BadRequest(this.ModelState);
          }
          
          User? user = await this._userService.GetUsernameAsync(command.Username);
          if (user?.Username != command.Username)
          {
               return BadRequest("Wrong username!");
          }

          if (!BCrypt.Net.BCrypt.Verify(command.Password, user.Password))
          {
               return BadRequest("Wrong password");
          }

          List<Claim> claims = new List<Claim>
          {
               new Claim(ClaimTypes.Name, user.Username)
          };
          var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(
               _secretKey));

          var cred = new SigningCredentials(key, 
               SecurityAlgorithms.HmacSha256Signature);
          var token = new JwtSecurityToken(
               claims: claims,
               expires: DateTime.Now.AddDays(1),
               signingCredentials: cred
          );

          var jwt = new JwtSecurityTokenHandler().WriteToken(token);
          

          return this.Ok(jwt);
     }
     
     [HttpPost]
     [Authorize]
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
     [Authorize]
     public async Task<IActionResult> DeleteUserAsync(int id)
     {
          await this._userService.DeleteAsync(id);
          return this.Ok();
     }
     

}
}