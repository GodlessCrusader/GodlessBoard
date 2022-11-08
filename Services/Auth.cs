using GodlessBoard.Models;
using GodlessBoard.Pages.Account;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using System.Security.Claims;

namespace GodlessBoard.Services
{
    public class Auth
    {
        private readonly JwtHandler _jwtHandler;
        public Auth(JwtHandler jwtHandler)
        {
            _jwtHandler = jwtHandler;
        }
        public Task SignInAsync(HttpContext httpContext, User user)
        {
            httpContext.Response.Cookies.Append("gboard_signin_token", _jwtHandler.GenetarateToken(user));
            return Task.CompletedTask;
        }
        public Task SignOutAsync(HttpContext httpContext)
        {
            if (!httpContext.Request.Cookies.Any(x => x.Key == "gboard_signin_token"))
                return Task.CompletedTask;
            httpContext.Response.Cookies.Delete("gboard_signin_token");
            return Task.CompletedTask;
        }
        
        public string GetUserName(string userIdentity)
        {
            return userIdentity.Split('|')[0];
        }
    }
}
