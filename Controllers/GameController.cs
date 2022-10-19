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
        private readonly MyDbContext _dbContext;
        public GameController(MyDbContext context)
        {
            _dbContext = context;
            
        }

        public string Sync(int id)
        {
            var result = "";
            if (HttpContext.User.Identity != null && HttpContext.User.Identity.Name != null && HttpContext.User.Identity.IsAuthenticated)
            {

                var user = _dbContext.Users.Single(x => x.UserName == Auth.GetUserName(HttpContext.User.Identity.Name));
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
