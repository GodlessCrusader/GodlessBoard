using GodlessBoard.Data;
using GodlessBoard.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace GodlessBoard.Pages.Account
{
    public class IndexModel : PageModel
    {
        public User? CurrentUser { get; set; }
        private readonly MyDbContext _context;
        public IndexModel(MyDbContext context)
        {
            _context = context;
        }
        public void OnGet()
        {
            if(User.Identity.IsAuthenticated)
            {
                CurrentUser = (from user in _context.Users
                              where user.UserName == User.Identity.Name
                              select user).SingleOrDefault();
                if(CurrentUser == null)
                {

                }
            }
            
        }
    }
}
