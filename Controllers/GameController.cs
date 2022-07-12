using GodlessBoard.Data;
using Microsoft.AspNetCore.Mvc;

namespace GodlessBoard.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class GameController : Controller
    {
        private readonly MyDbContext _context;
        public GameController(MyDbContext context)
        {
            _context = context;
        }
        
        public IActionResult Join(string id)
        {
            return Ok();
        }
        public IActionResult Index()
        {
            return View();
        }
    }
}
