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
        private readonly MyDbContext _dbContext;

        private readonly JwtHandler _jwtHandler;

        private readonly MediaUploadRouter _mediaUploadRouter;
        public MediaUploadController(MyDbContext context, JwtHandler jwtHandler, MediaUploadRouter mediaUploadRouter)
        {
            _dbContext = context;
            _jwtHandler = jwtHandler;
            _mediaUploadRouter = mediaUploadRouter;
        }

        [HttpGet]
        public IActionResult Index()
        {
            return RedirectToPage("/Account/Index");

        }

        [HttpPost]
        public IActionResult UploadImage(object image)
        {
            if (!Request.Headers.Any(x => x.Key == "gboard_signin_token"))
                return Unauthorized();
            
            var token = Request.Headers.First(x => x.Key == "gboard_signin_token");
            
            if (!_jwtHandler.ValidateToken(token.Value))
                return Unauthorized();

            _mediaUploadRouter.

            return Ok();
        }
        
    }
}
