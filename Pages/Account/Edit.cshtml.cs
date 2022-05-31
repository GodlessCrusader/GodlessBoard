using GodlessBoard.Data;
using GodlessBoard.Models;
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
                CurrentUser = (from user in _context.Users
                               where user.UserName == User.Identity.Name
                               select user).SingleOrDefault();
                if (CurrentUser == null)
                {

                }
            }
        }
    }
}
