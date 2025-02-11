using Microsoft.AspNetCore.Cors;
using Microsoft.AspNetCore.Mvc;

namespace TodoApp.API.Controllers
{
    [EnableCors("CorsPolicy")]  
    [ApiController]
    [Route("api/[controller]")]
    public abstract class BaseController : ControllerBase
    {
    }
} 