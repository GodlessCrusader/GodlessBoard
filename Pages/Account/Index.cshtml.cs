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
        private readonly Auth _auth;
        public IndexModel(MyDbContext context, Auth auth)
        {
            _context = context;
            _auth = auth;
        }
        public void OnGet()
        {
            if(User.Identity.IsAuthenticated)
            {
                var userName = User.Claims.First(x => x.Type == ClaimTypes.Email).Value;
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
