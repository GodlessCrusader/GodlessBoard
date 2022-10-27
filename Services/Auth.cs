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
        public async Task IdentifyAsync(HttpContext httpContext, User user)
        {
            httpContext.Response.Cookies.Append("gboard_signin_token", _jwtHandler.GenetarateToken(user));
            
            
        }

        public void ParseIdentityName(string userIdentity, out string Email, out string DisplayName)
        {
            var words = userIdentity.Split('|');
            Email = words[0];
            DisplayName = words[1];
        }
        public string GetUserName(string userIdentity)
        {
            return userIdentity.Split('|')[0];
        }
    }
}
