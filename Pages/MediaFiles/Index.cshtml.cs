using GodlessBoard.Data;
using GodlessBoard.Models;
using GodlessBoard.Services;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;

namespace GodlessBoard.Pages.MediaFiles
{
    public class IndexModel : PageModel
    {
        private MyDbContext _context;
        public User CurrentUser { get; set; }
        public List<Media> Images { get; set; } = new List<Media>();

        public List<Media> Audios { get; set; } = new List<Media>();

        public IndexModel(MyDbContext context)
        {
            _context = context; 
            
        }
        
        public void OnGet()
        {
            if(HttpContext.User.Identity.IsAuthenticated)
            {
                CurrentUser = _context.Users.Where(x => x.UserName == Auth.GetUserName(User.Identity.Name)).Single();
                Images = _context.Media.Where(x => x.OwnerId == CurrentUser.Id && x.Type == MediaType.Image).ToList();
                Audios = _context.Media.Where(x => x.OwnerId == CurrentUser.Id && x.Type == MediaType.Audio).ToList();
            }
            
        }
    }
}
