using Microsoft.AspNetCore.Mvc;

namespace RavelDev.PensandoPeliculas.WebApi.Controllers
{
    public class BaseController : ControllerBase
    {
        private IConfiguration _config;

        protected string ipAddress()
        {
            if (Request.Headers.ContainsKey("X-Forwarded-For"))
                return Request.Headers["X-Forwarded-For"];
            else
                return HttpContext.Connection.RemoteIpAddress.MapToIPv4().ToString();
        }
    }
}
