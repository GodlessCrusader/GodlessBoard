using GodlessBoard.Data;
using GodlessBoard.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace GodlessBoard.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class UserController : Controller
    {
        private readonly MyDbContext _context;
        public UserController(MyDbContext context)
        {
            _context = context;
        }

        [HttpGet]
        public User Get()
        {
            return new User();
           
        }
       
    }
}
