using GodlessBoard.Pages.Account;
using Microsoft.AspNetCore.Authentication;
using System.Security.Claims;

namespace GodlessBoard.Services
{
    public class Auth
    {
        public static async Task Identify(HttpContext HttpContext, string UserName, string DisplayName)
        {
            var claims = new List<Claim>
            {
                new Claim(ClaimTypes.Email, UserName.ToUpper()),
                new Claim(ClaimTypes.Name, DisplayName)
            };
            var identity = new ClaimsIdentity(claims, "GodlessCookie");
            ClaimsPrincipal claimsPrincipal = new ClaimsPrincipal(identity);
            await HttpContext.SignInAsync("GodlessCookie", claimsPrincipal);
        }
    }
}
