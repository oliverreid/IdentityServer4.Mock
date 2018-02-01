using System.Linq;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace BookingApi.Web.Controllers
{
    [Route("api")]
    public class PingController : Controller
    {
        [HttpGet]
        [Route("ping")]
        public string Ping()
        {
            return "Pong";
        }

        [Authorize]
        [HttpGet]
        [Route("ping/secure")]
        public string PingSecured()
        {
            return "Authenticated";
        }

        [Authorize]
        [HttpGet("claims")]
        public object Claims()
        {
            return User.Claims.Select(c =>
            new
            {
                Type = c.Type,
                Value = c.Value
            });
        }
    }
}