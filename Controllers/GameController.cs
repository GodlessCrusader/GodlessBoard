using GodlessBoard.Data;
using GodlessBoard.Services;
using Microsoft.AspNetCore.Cors;
using Microsoft.AspNetCore.Mvc;
using System.Linq;
using System.Net.WebSockets;
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
     
        private bool AuthorizeRequest()
        {
            return true;
        }
        public string Sync(int id)
        {
           
            if(!Request.Headers.Any(x => x.Key == "gboard_signin_token"))
                return string.Empty;
            var token = Request.Headers.First(x => x.Key == "gboard_signin_token").Value;
            
            var result = "";
            
            if(!_jwtHandler.ValidateToken(token))
                return string.Empty;
            var claims = _jwtHandler.GetClaims(token);
            var userName = claims.First(x => x.Type == ClaimValueTypes.Email).Value;
            var user = _dbContext.Users.First(x => x.UserName == userName);
            if(user != null && _dbContext.UserGameRole.Any(x => x.UserId == user.Id && x.GameId == id))
            {
                var game = _dbContext.Games.First(x => x.Id == id);
                result = game.JsonRepresentation;
            }
            return result;
            //var authCookie = httpContext.Request.Headers.Cookie.SingleOrDefault();
            
        }

        public IActionResult Chat(int gameId)
        {
            if (!Request.Headers.Any(x => x.Key == "gboard_signin_token"))
                return Unauthorized();
            var token = Request.Headers.First(x => x.Key == "gboard_signin_token").Value;

            if (!_jwtHandler.ValidateToken(token))
                return Unauthorized();
            var claims = _jwtHandler.GetClaims(token);
            var userName = claims.First(x => x.Type == ClaimValueTypes.Email).Value;
            var user = _dbContext.Users.First(x => x.UserName == userName);
            if (user != null && _dbContext.UserGameRole.Any(x => x.UserId == user.Id && x.GameId == gameId))
            {
                var chat = _dbContext.Messages.Where(x => x.Game.Id == gameId);
                return Ok(chat);
            }
            return BadRequest();
        }
        public IActionResult Join(string id)
        {
            return Ok();
        }
  
    }
}
