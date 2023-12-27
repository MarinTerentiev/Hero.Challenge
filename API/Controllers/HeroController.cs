using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace WebAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class HeroController : ApiControllerBase
    {
        [HttpGet(Name = "HealthCheck")]
        public ActionResult<string> HealthCheck()
        {
            return "Health Check";
        }
    }
}
