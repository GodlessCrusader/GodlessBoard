using GodlessBoard.Data;
using GodlessBoard.Models;
using GodlessBoard.Services;
using Microsoft.AspNetCore.Components.Forms;
using Microsoft.AspNetCore.Mvc;

namespace GodlessBoard.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class MediaUploadController : ControllerBase
    {
        private readonly MyDbContext _context;
        public MediaUploadController(MyDbContext context)
        {
            _context = context;
        }

        [HttpGet]
        public IActionResult Index()
        {
            return RedirectToPage("/Account/Index");

        }
        
    }
}
