using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using AlgebraImageApp.Logger;
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
     
     [HttpGet("u/{username}")]
     [AllowAnonymous]
     public async Task<IActionResult> FetchUserAsync(string username)
     {
          User? user = await this._userService.GetUsernameAsync(username);

          if (user is null)
          {
               return this.NotFound();
          }
          
          LoggerSingleton.Instance.Log($"Successfully retrieved user {username}.");
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
          
          // Create a custom response object
          var response = new
          {
               userType = user.Type,
               userTier = user.Tier,
               userId = user.Id,
               status = "success",
               message = "logged in successfully",
               data = new { username = command.Username },
               accessToken = jwt,
               username = command.Username,
               user= new
               {
                    username = command.Username
               }
          };


          return this.Ok(response);
     }
     
     [HttpPost("logout")]
     [Authorize] // Add the Authorize attribute to ensure the user is authenticated
     public IActionResult Logout()
     {
          return Ok();
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
          
     [HttpGet("consumption/{id}")]
     [AllowAnonymous]
     public async Task<IActionResult> GetUserConsumption(int id)
     {
          int consumption = await this._userService.GetConsumption(id);
          return this.Ok(consumption);
     }

}
}