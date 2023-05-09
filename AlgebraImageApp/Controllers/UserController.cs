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
     
}
}