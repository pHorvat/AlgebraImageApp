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
using Newtonsoft.Json;
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
     private readonly string _refreshSecretKey = "z8GUYJve6tzvEx0yAPaAGQxsfMuKTxHCaF82fYbJ";
     public ICounter userActions = Metrics.DefaultFactory.CreateCounter("UserActions", "Number of user actions");
     public IGauge usersLoggedIn = Metrics.DefaultFactory.CreateGauge("UsersLoggedIn", "Number of user currently logged in");
     private static List<string> refreshTokens = new List<string>();


     public UserController(IUserService userService)
     {
          //SOLID:  Dependency Inversion Principle,
          //as the high-level class (UserController) depends on an abstraction (IUserService) rather than a concrete implementation
          this._userService = userService;
          refreshTokens.Add("test123");
     }
     
     private bool IsValid(string token)
     {
          JwtSecurityToken jwtSecurityToken;
          try
          {
               jwtSecurityToken = new JwtSecurityToken(token);
          }
          catch (Exception)
          {
               return false;
          }
    
          return jwtSecurityToken.ValidTo > DateTime.UtcNow;
     }

     [HttpGet]
     [Authorize]
     [LoggingAspect]
     public async Task<IActionResult> GetAllUsersAsync()
     {
          IEnumerable<User> user = await this._userService.GetAllAsync();
          return this.Ok(user);
     }
     
     
     [HttpGet("{id}")]
     [Authorize]
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
     [Authorize]
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
               //return BadRequest("Wrong username!");
               return BadRequest("Wrong username or password!");
          }

          if (!BCrypt.Net.BCrypt.Verify(command.Password, user.Password))
          {
               //return BadRequest("Wrong password");
               return BadRequest("Wrong username or password!");

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
               expires: DateTime.Now.AddSeconds(5),
               signingCredentials: cred
          );
          
          var refreshKey =new SymmetricSecurityKey(Encoding.UTF8.GetBytes(
               _refreshSecretKey));
          var refreshCred = new SigningCredentials(refreshKey, 
               SecurityAlgorithms.HmacSha256Signature);
          var refreshToken = new JwtSecurityToken(
               expires: DateTime.Now.AddDays(7),
               signingCredentials: refreshCred
          );

          var jwt = new JwtSecurityTokenHandler().WriteToken(token);
          var refreshJwt = new JwtSecurityTokenHandler().WriteToken(refreshToken);
          Console.WriteLine(refreshJwt);
          refreshTokens.Add(refreshJwt);
          Console.WriteLine(refreshTokens.Count);
          
          
          var response = new
          {
               //userPassword=user.Password,
               userType = user.Type,
               userTier = user.Tier,
               userId = user.Id,
               status = "success",
               message = "logged in successfully",
               data = new { username = command.Username },
               accessToken = jwt,
               refreshToken=refreshJwt,
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
     [Authorize] 
     public IActionResult Logout()
     {
          userActions.Inc(1);
          usersLoggedIn.Dec();
          return Ok();
     }
     
     [HttpPost("refresh")]
     [AllowAnonymous] 
     public IActionResult RefreshToken()
     {
          string refreshToken = Request.Form["token"];
          Console.WriteLine(refreshToken);
          Console.WriteLine(refreshTokens.Count);
          foreach(var month in refreshTokens)
          {
               Console.WriteLine(month);
          }

          var handler = new JwtSecurityTokenHandler();
          var jsonToken = handler.ReadToken(refreshToken) as JwtSecurityToken;
          if (refreshTokens.Contains(refreshToken))
          {
               if (!IsValid(refreshToken))
               {
                    Console.WriteLine("Expired token");
                    return BadRequest("Expired or invalid token");
               }
               else
               {
                    var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(
                         _secretKey));

                    var cred = new SigningCredentials(key, 
                         SecurityAlgorithms.HmacSha256Signature);
                    var token = new JwtSecurityToken(
                         expires: DateTime.Now.AddHours(1),
                         signingCredentials: cred
                    );
                    var jwt = new JwtSecurityTokenHandler().WriteToken(token);
                    var response = new
                    {
                         accessToken = jwt
                    };
                    return Ok(response);

               }
          }
          
          Console.WriteLine("No token in list");
          return BadRequest("Expired or invalid token");
     }
     
     [HttpPost]
     [Authorize]
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
     [Authorize]
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
     [Authorize]
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
     [Authorize]
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