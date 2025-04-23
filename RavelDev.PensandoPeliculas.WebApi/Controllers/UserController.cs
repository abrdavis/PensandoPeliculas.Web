using Azure;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using RavelDev.Core.Models.Authentication;
using RavelDev.PensandoPeliculas.Users;
using RavelDev.PensandoPeliculas.WebApi.Models.Auth;
using static RavelDev.PensandoPeliculas.WebApi.Utility.Authentication.AuthConstants;

namespace RavelDev.PensandoPeliculas.WebApi.Controllers
{
    [Route("[controller]")]
    public class UserController : BaseController
    {
        public UserController(IUserService userService, IUserManager uesrManager)
        {
            UserService = userService;
            UserManager = uesrManager;
        }

        public IUserService UserService { get; }
        public IUserManager UserManager { get; }


        [HttpPost("Login")]
        [AllowAnonymous]
        public IActionResult Login([FromBody] LoginRequest loginRequest)
        {
            try
            {
                if (string.IsNullOrEmpty(loginRequest.Username) || string.IsNullOrEmpty(loginRequest.Password))
                {
                    return new JsonResult(new { success = false, loginInfoIncomplete = true });
                }
                var userData = UserService.Authenticate(loginRequest.Username, loginRequest.Password, ipAddress());

                if (userData == null)
                    return BadRequest(new { message = "Username or password is incorrect" });

                SetCookies(userData);

                return new JsonResult(new { user = userData.UserName, success = true });
            }
            catch (Exception ex)
            {
                return new JsonResult(new { error = ex.Message });
            }
        }

        [HttpGet("Logout")]
        [AllowAnonymous]
        public IActionResult Logout()
        {
            try
            {
                ClearCookies();
                return new JsonResult(new { success = true });
            }
            catch (Exception ex)
            {
                Console.WriteLine("Error logging out.", ex);
                return new JsonResult(new { error = ex.Message });
            }
        }

        private void ClearCookies()
        {
            Response.Cookies.Delete(AuthCookieNames.AcecssToken);
            Response.Cookies.Delete(AuthCookieNames.UserId);
            Response.Cookies.Delete(AuthCookieNames.RefreshToken);
        }

        [HttpGet("CheckPermissions")]
        [Authorize]
        public IActionResult CheckPermissions(string permissionType)
        {
            try
            {
                //var roles = RoleRepositry.
                return new JsonResult(new { user = string.Empty, success = true });
            }
            catch(Exception ex)
            {
                return new JsonResult(new { error = ex.Message });
            } 
        }

        private void SetCookies(AuthenticateResponse authData)
        {
            Response.Cookies.Append(AuthCookieNames.AcecssToken, $"{authData.Token}", new CookieOptions() { HttpOnly = true, SameSite = SameSiteMode.Strict, Secure = true, Expires = DateTime.UtcNow.AddMinutes(50) });
            Response.Cookies.Append(AuthCookieNames.UserId, authData.Id.ToString(), new CookieOptions() { HttpOnly = true, SameSite = SameSiteMode.Strict, Secure = true, Expires = DateTime.UtcNow.AddMinutes(50) });
            Response.Cookies.Append(AuthCookieNames.RefreshToken, authData.RefreshToken, new CookieOptions() { HttpOnly = true, SameSite = SameSiteMode.Strict, Secure = true, Expires = DateTime.UtcNow.AddMinutes(50) });
            Response.Cookies.Append(AuthCookieNames.TokenValid, (true).ToString(), new CookieOptions() { HttpOnly = false, SameSite = SameSiteMode.Strict, Secure = true, Expires = DateTime.UtcNow.AddMinutes(50) });

        }
    }
}
