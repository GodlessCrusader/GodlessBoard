using GodlessBoard.Data;
using GodlessBoard.Models;
using GodlessBoard.Services;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using System.Security.Claims;

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
                string userName, displayName;
                Auth.ParseIdentityName(User.Identity.Name, out userName, out displayName);
                CurrentUser = (from currentUser in _context.Users
                              where currentUser.UserName == userName
                              select currentUser).SingleOrDefault();
                if(CurrentUser == null)
                {

                }
            }
            
        }
    }
}
