using GameModel;
using GodlessBoard.Data;
using GodlessBoard.Models;
using GodlessBoard.Services;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using System.Security.Claims;

namespace GodlessBoard.Pages.MediaFiles
{
    public class IndexModel : PageModel
    {
        private MyDbContext _context;
        private readonly Auth _auth;
        public IndexModel(MyDbContext context, Auth auth)
        {
            _auth = auth;
            _context = context;

        }

        public User? CurrentUser { get; set; }
        public List<Media> Images { get; set; } = new List<Media>();

        public List<Media> Audios { get; set; } = new List<Media>();

        
        
        public void OnGet()
        {
            if(HttpContext.User.Identity.IsAuthenticated)
            {
                var userName = User.Claims.First(x => x.Type == ClaimTypes.Email).Value;
                CurrentUser = _context.Users.First(x => x.UserName == userName);
                Images = _context.Media.Where(x => x.OwnerId == CurrentUser.Id && x.Type == MediaType.Image).ToList();
                Audios = _context.Media.Where(x => x.OwnerId == CurrentUser.Id && x.Type == MediaType.Audio).ToList();
            }
            
        }
    }
}
