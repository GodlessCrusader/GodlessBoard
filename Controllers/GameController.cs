using GodlessBoard.Data;
using Microsoft.AspNetCore.Cors;
using Microsoft.AspNetCore.Mvc;

namespace GodlessBoard.Controllers
{
    [ApiController]
    
    [Route("[controller]/[action]/{id}")]
    public class GameController : Controller
    {
        private readonly MyDbContext _context;
        public GameController(MyDbContext context)
        {
            _context = context;
        }
        
        public string Sync(int id)
        {
            var result = _context.Games.Where(x => x.Id == id).Single().JsonRepresentation;
            return result;
        }
        public IActionResult Join(string id)
        {
            return Ok();
        }
  
    }
}
