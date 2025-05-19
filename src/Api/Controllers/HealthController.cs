using System.Web.Http;

namespace Api.Controllers
{
    [AllowAnonymous]
    [RoutePrefix("api/health")]
    public class HealthController : ApiController
    {
        [HttpGet, Route("")]
        public IHttpActionResult Get() => Ok("Healthy");
    }
}