using System.Diagnostics.Metrics;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using AlgebraImageApp.Aspect;
using AlgebraImageApp.Models;
using AlgebraImageApp.Models.Commands;
using AlgebraImageApp.Patterns;
using AlgebraImageApp.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Tokens;
using Prometheus;
using Prometheus.Client;

namespace AlgebraImageApp.Controllers
{

[Route("api/[controller]")]
[ApiController]
[Authorize]
public class UserController : ControllerBase
{
     private IUserService _userService;
     private readonly string _secretKey = "ZcK#K5KdDtq8Bx%%ByhKg9BhUrtw^M6aXrnUYwQEPWn9";
     public ICounter userActions = Metrics.DefaultFactory.CreateCounter("UserActions", "Number of user actions");
     public IGauge usersLoggedIn = Metrics.DefaultFactory.CreateGauge("UsersLoggedIn", "Number of user currently logged in");
     

     


     public UserController(IUserService userService)
     {
          //SOLID:  Dependency Inversion Principle,
          //as the high-level class (UserController) depends on an abstraction (IUserService) rather than a concrete implementation
          this._userService = userService;
          
     }

     [HttpGet]
     [AllowAnonymous]
     [LoggingAspect]
     public async Task<IActionResult> GetAllUsersAsync()
     {
          IEnumerable<User> user = await this._userService.GetAllAsync();
          return this.Ok(user);
     }
     
     
     [HttpGet("{id}")]
     [AllowAnonymous]
     [LoggingAspect]
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
     [LoggingAspect]
     public async Task<IActionResult> FetchUserAsync(string username)
     {
          User? user = await this._userService.GetUsernameAsync(username);

          if (user is null)
          {
               return this.NotFound();
          }
          
          return this.Ok(user);
     }
     
     
     [HttpPost("register")]
     [AllowAnonymous]
     [LoggingAspect]
     public async Task<IActionResult> RegisterAsync(CreateUserCommand command)
     {
          userActions.Inc(1);
          if (this.ModelState.IsValid == false)
          {
               return this.BadRequest(this.ModelState);
          }

          string passwordHash = BCrypt.Net.BCrypt.HashPassword(command.Password);
          command.Password = passwordHash;
          
          
          int id = await this._userService.CreateAsync(command);
          CustomLogger.Instance.Log(command.Username + " has been registered.");
          return this.Ok(id);
     }
     
     [HttpPost("login")]
     [AllowAnonymous]
     [LoggingAspect]
     public async Task<IActionResult> LoginAsync(CreateUserCommand command)
     {
          userActions.Inc(1);
          usersLoggedIn.Inc();
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
               userPassword=user.Password,
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

          CustomLogger.Instance.Log(command.Username + " has logged in.");
          return this.Ok(response);
     }
     
     
     [HttpPost("logout")]
     [AllowAnonymous] 
     public IActionResult Logout()
     {
          userActions.Inc(1);
          usersLoggedIn.Dec();
          return Ok();
     }
     
     [HttpPost]
     [AllowAnonymous]
     //[Authorize]
     public async Task<IActionResult> CreateUserAsync(CreateUserCommand command)
     {
          userActions.Inc(1);
          if (this.ModelState.IsValid == false)
          {
               return this.BadRequest(this.ModelState);
          }
          
          CustomLogger.Instance.Log(command.Username + " has been created.");

          int id = await this._userService.CreateAsync(command);
          return this.Ok(id);
     }
     
     [HttpPut]
     [AllowAnonymous]
     //[Authorize]
     public async Task<IActionResult> UpdateUserAsync(UpdateUserCommand command)
     {
          userActions.Inc(1);
          if (this.ModelState.IsValid == false)
          {
               return this.BadRequest(this.ModelState);
          }

          try
          {
               await this._userService.UpdateAsync(command);
               CustomLogger.Instance.Log(command.Username + " has been updated.");
               return this.Ok();
          }
          catch (ArgumentNullException ex)
          {
               return this.BadRequest(ex.Message);
          }
     }
     
     [HttpPut ("updateTier")]
     [AllowAnonymous]
     //[Authorize]
     public async Task<IActionResult> UpdateUserTierAsync(UpdateUserCommand command)
     {
          userActions.Inc(1);
          if (this.ModelState.IsValid == false)
          {
               return this.BadRequest(this.ModelState);
          }

          try
          {
               await this._userService.UpdateAsync(command);
               var userTierSubject = new UserTierSubject();
               var userTierObserver = new UserTierObserver(_userService);
               userTierSubject.Attach(userTierObserver);

               userTierSubject.Notify(command.Id);
               CustomLogger.Instance.Log(command.Username + " has updated the tier to "+command.Tier);

               return this.Ok();
          }
          catch (ArgumentNullException ex)
          {
               return this.BadRequest(ex.Message);
          }
     }
     
     
     [HttpDelete("{id}")]
     [AllowAnonymous]
     //[Authorize]
     public async Task<IActionResult> DeleteUserAsync(int id)
     {
          userActions.Inc(1);
          await this._userService.DeleteAsync(id);
          CustomLogger.Instance.Log("User with id "+ id +" has been deleted.");

          return this.Ok();
     }
          
     [HttpGet("consumption/{id}")]
     [AllowAnonymous]
     public async Task<IActionResult> GetUserConsumption(string id)
     {
          int consumption = await this._userService.GetConsumption(id);
          return this.Ok(consumption);
     }

}
}