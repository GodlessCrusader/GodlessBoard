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
        public IndexModel(MyDbContext context)
        {
            _context = context; 
        }
        public User CurrentUser { get; set; }
        public void OnGet()
        {
            if(HttpContext.User.Identity.IsAuthenticated)
            {
                CurrentUser = _context.Users.Where(x => x.UserName == Auth.GetUserName(User.Identity.Name)).Single();
            }
            
        }
    }
}
