using GodlessBoard.Data;
using GodlessBoard.Services;
using Microsoft.AspNetCore.Cors;
using Microsoft.AspNetCore.Mvc;
using System.Linq;

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
            var result = "";
            var cookie = HttpContext.Request.Cookies.First(x => x.Key == "gboard_signin_token");
            
            if (HttpContext.User.Identity != null && HttpContext.User.Identity.Name != null && HttpContext.User.Identity.IsAuthenticated)
            {

                var user = _dbContext.Users.Single(x => x.UserName == _auth.GetUserName(HttpContext.User.Identity.Name));
                var allowedUsers = (from users in _dbContext.UserGameRole
                                    where users.GameId == id
                                    select users.UserId);
                if (user != null && allowedUsers.Contains(user.Id))
                {
                    result = _dbContext.Games.Where(x => x.Id == id).Single().JsonRepresentation;
                }
                else
                    result = "user isn't allowed";
            }
            else
                result = "Authentification problem"; 
            return result;
            //var authCookie = httpContext.Request.Headers.Cookie.SingleOrDefault();
            
        }
        public IActionResult Join(string id)
        {
            return Ok();
        }
  
    }
}
