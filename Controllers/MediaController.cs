using GameModel;
using GodlessBoard.Data;
using GodlessBoard.Models;
using GodlessBoard.Services;
using Microsoft.AspNetCore.Components.Forms;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Net.Mime;
using System.Security.Claims;

namespace GodlessBoard.Controllers
{
    [ApiController]
    [Route("[controller]/[action]/{id}")]
    public class MediaController : ControllerBase
    {
        private readonly MyDbContext _dbContext;

        private readonly JwtHandler _jwtHandler;

        private readonly MediaUploadRouter _mediaUploadRouter;
        public MediaController(MyDbContext context, JwtHandler jwtHandler, MediaUploadRouter mediaUploadRouter)
        {
            _dbContext = context;
            _jwtHandler = jwtHandler;
            _mediaUploadRouter = mediaUploadRouter;
        }

        private async Task<User> AuthorizeRequestAsync()
        {
            if (!Request.Headers.Any(x => x.Key == "gboard_signin_token"))
                return null;
            var token = Request.Headers.First(x => x.Key == "gboard_signin_token");

            if (!_jwtHandler.ValidateToken(token.Value))
                return null;

            var userName = _jwtHandler.GetClaims(token.Value).First(x => x.Type == ClaimTypes.Email).Value;

            if(! await _dbContext.Users.AnyAsync())
            {
                return null;
            }

            return await _dbContext.Users.FirstAsync(x => x.UserName == userName);
        }

        [HttpGet]
        public IActionResult Index()
        {
            return RedirectToPage("/Account/Index");

        }

        [HttpGet]
        public async Task<IActionResult> Remove(long id)
        {
            var user = await AuthorizeRequestAsync();

            if (user == null)
                return Unauthorized();

            
            if(! await _dbContext.Media.AnyAsync(x => x.Id == id && x.OwnerId == user.Id))
            {
                return BadRequest();
            }

            var media = await _dbContext.Media.FirstAsync(x => x.Id == id);

            await _mediaUploadRouter.DeleteAsync(media);

            _dbContext.Media.Remove(media);

            await _dbContext.SaveChangesAsync();

            

            return Ok();
        }

        [HttpPost]
        public async Task<IActionResult> Upload([FromBody]byte [] bytes, string json)
        {

            var user = await AuthorizeRequestAsync();

            if (user == null)
                return Unauthorized();

           var file = Newtonsoft.Json.JsonConvert.DeserializeObject<MediaFile>(json);

            if (_mediaUploadRouter.CheckExistance(bytes, user.UserName, file.UserDisplayName))
                return Ok();
            
            var media = await _mediaUploadRouter.UploadMediaAsync(bytes, user.UserName, file.UserDisplayName);
            media.Owner = user;
            media.OwnerId = user.Id;
            media.Weight = file.Size;
            _dbContext.Media.Add(media);
            await _dbContext.SaveChangesAsync();
            
            return Ok();
        }

        
    }
}
