using GodlessBoard.Models;
using Microsoft.AspNetCore.Authentication;
using Microsoft.Extensions.Options;
using System.Collections.Generic;
using System.Security.Claims;
using System.Text.Encodings.Web;

namespace GodlessBoard.Services
{
    public class AuthHandler : AuthenticationHandler<AuthOptions>
    {
        private JwtHandler _jwtHandler;

        public const string PolicyName = "AuthPolicy";
        public AuthHandler(IOptionsMonitor<AuthOptions> options, ILoggerFactory logger, UrlEncoder encoder, ISystemClock clock, JwtHandler jwtHandler)
            : base(options, logger, encoder, clock)
        {
            _jwtHandler = jwtHandler;
        }

        protected override async Task<AuthenticateResult> HandleAuthenticateAsync()
        {
            if (!Request.Cookies.Any(x => x.Key == "gboard_signin_token"))
                return AuthenticateResult.NoResult();
            
            var cookie = Request.Cookies.First(x => x.Key == "gboard_signin_token");

            if(_jwtHandler.GetClaims(cookie.Value) == null)
                return AuthenticateResult.NoResult();
            
            List<Claim> claims = _jwtHandler.GetClaims(cookie.Value).ToList();
            ClaimsIdentity identity = new(claims, Scheme.Name);
            ClaimsPrincipal claimsPrincipal = new(identity);
            return AuthenticateResult.Success(new AuthenticationTicket(claimsPrincipal, Scheme.Name));
            
        }
    }
}
