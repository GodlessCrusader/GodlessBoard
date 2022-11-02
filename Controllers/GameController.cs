using GodlessBoard.Data;
using GodlessBoard.Services;
using Microsoft.AspNetCore.Cors;
using Microsoft.AspNetCore.Mvc;
using System.Linq;
using System.Security.Claims;

namespace GodlessBoard.Controllers
{
    [ApiController]
    
    [Route("[controller]/[action]/{id}")]
    public class GameController : Controller
    {
        private readonly Auth _auth;
        private readonly JwtHandler _jwtHandler;
        private readonly MyDbContext _dbContext;
        public GameController(MyDbContext context, JwtHandler jwtHandler, Auth auth)
        {
            _auth = auth;
            _dbContext = context;
            _jwtHandler = jwtHandler;
        }
     
        public string Sync(int id)
        {
            if(HttpContext.Request.Cookies.Any(x => x.Key == "gboard_signin_token"))
                return string.Empty;
            var result = "";
            var cookie = HttpContext.Request.Cookies.First(x => x.Key == "gboard_signin_token");
            if(!_jwtHandler.ValidateToken(cookie.Value))
                return string.Empty;
            var claims = _jwtHandler.GetClaims(cookie.Value);
            var user = _dbContext.Users.First(x => x.UserName == claims.First(x => x.ValueType == ClaimValueTypes.Email).Value);
            if(user != null && _dbContext.UserGameRole.Any(x => x.UserId == user.Id && x.GameId == id))
            {
                var game = _dbContext.Games.First(x => x.Id == id);
                result = game.JsonRepresentation;
            }
            return result;
            //var authCookie = httpContext.Request.Headers.Cookie.SingleOrDefault();
            
        }
        public IActionResult Join(string id)
        {
            return Ok();
        }
  
    }
}
