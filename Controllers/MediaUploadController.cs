using GodlessBoard.Data;
using GodlessBoard.Models;
using GodlessBoard.Services;
using Microsoft.AspNetCore.Components.Forms;
using Microsoft.AspNetCore.Mvc;
using System.Net.Mime;
using System.Security.Claims;

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
        public async Task<IActionResult> UploadImage(IFormFile file)
        {
            if (!Request.Headers.Any(x => x.Key == "gboard_signin_token"))
                return Unauthorized();
            
            var token = Request.Headers.First(x => x.Key == "gboard_signin_token");
            
            if (!_jwtHandler.ValidateToken(token.Value))
                return Unauthorized();

            var userName = _jwtHandler.GetClaims(token.Value).First(x => x.Type == ClaimTypes.Email).Value;

            using MemoryStream ms = new();

            file.CopyTo(ms);

            if (_mediaUploadRouter.CheckExistance(ms.ToArray(), userName, file.FileName))
                return Ok();

            await _mediaUploadRouter.UploadMediaAsync(ms.ToArray(), userName, file.FileName);
            
            return Ok();
        }
        
    }
}
