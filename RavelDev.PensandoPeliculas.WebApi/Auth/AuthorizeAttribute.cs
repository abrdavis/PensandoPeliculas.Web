
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using RavelDev.PensandoPeliculas.Users.Models;
using static RavelDev.PensandoPeliculas.WebApi.Utility.Authentication.AuthConstants;


[AttributeUsage(AttributeTargets.Class | AttributeTargets.Method)]
public class AuthorizeAttribute : Attribute, IAuthorizationFilter
{
    public void OnAuthorization(AuthorizationFilterContext context)
    {
        // skip authorization if action is decorated with [AllowAnonymous] attribute
        var allowAnonymous = context.ActionDescriptor.EndpointMetadata.OfType<AllowAnonymousAttribute>().Any();
        if (allowAnonymous)
            return;

        // authorization
        var user = (UserModel?)context.HttpContext.Items["User"];
        if (user == null)
        {
            var tokenCookieNotFound = true;
            if (context.HttpContext.Request.Cookies.ContainsKey(AuthCookieNames.AcecssToken))
            {
                tokenCookieNotFound = false;
            }
            context.Result = new JsonResult(new { message = "Unauthorized", tokenCookieNotFound = tokenCookieNotFound }) { StatusCode = StatusCodes.Status401Unauthorized };
        }
    }
}