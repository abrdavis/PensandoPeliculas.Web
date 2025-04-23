using Microsoft.AspNetCore.Http;
using Newtonsoft.Json;
using RavelDev.PensandoPeliculas.Users;
using System.Net;

public class JwtMiddleware
{
    private readonly RequestDelegate _next;

    public JwtMiddleware(RequestDelegate next)
    {
        _next = next;
    }

    public async Task Invoke(HttpContext context, IUserService userService)
    {
        var token = context.Request.Cookies["X-Access-Token"];
        var endpoint = context.GetEndpoint();


        if (!string.IsNullOrEmpty(token))
        {
            var userId = userService.ValidateJwtToken(token);
            if (userId != null)
            {
                // attach user to context on successful jwt validation
                context.Items["User"] = userService.GetById(userId.Value);
            }
        }

        await _next(context);
    }
}