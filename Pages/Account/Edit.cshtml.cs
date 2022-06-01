using GodlessBoard.Data;
using GodlessBoard.Models;
using GodlessBoard.Services;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace GodlessBoard.Pages.Account
{
    public class EditModel : PageModel
    {
        public User? CurrentUser { get; set; }
        private readonly MyDbContext _context;
        public EditModel(MyDbContext context)
        {
            _context = context;
        }
        public void OnGet()
        {
            if (User.Identity.IsAuthenticated)
            {
                string userName, displayName;
                Auth.ParseIdentityName(User.Identity.Name, out userName, out displayName);
                CurrentUser = (from user in _context.Users
                               where user.UserName == userName
                               select user).SingleOrDefault();
                if (CurrentUser == null)
                {

                }
            }
        }
    }
}
