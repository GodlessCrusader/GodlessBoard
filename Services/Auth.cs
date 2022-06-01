using GodlessBoard.Models;
using GodlessBoard.Pages.Account;
using Microsoft.AspNetCore.Authentication;
using System.Security.Claims;

namespace GodlessBoard.Services
{
    public class Auth
    {
        public static async Task Identify(HttpContext httpContext, string userName, string displayName)
        {
            var claims = new List<Claim>
            {
                new Claim(ClaimTypes.Name, $"{userName.ToUpper()}|{displayName}"),
            };
            var identity = new ClaimsIdentity(claims, "GodlessCookie");
            ClaimsPrincipal claimsPrincipal = new ClaimsPrincipal(identity);
            await httpContext.SignInAsync("GodlessCookie", claimsPrincipal);
        }

        public static void ParseIdentityName(string userIdentity, out string Email, out string DisplayName)
        {
            var words = userIdentity.Split('|');
            Email = words[0];
            DisplayName = words[1];
        }
    }
}
