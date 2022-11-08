using GodlessBoard.Data;
using GodlessBoard.Models;
using GodlessBoard.Services;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using System.Security.Claims;

namespace GodlessBoard.Pages.Account
{
    public class EditModel : PageModel
    {
        public User? CurrentUser { get; set; }
        private readonly MyDbContext _context;
        private readonly Auth _auth;
        public EditModel(MyDbContext context, Auth auth)
        {
            _context = context;
            _auth = auth;
        }
        public void OnGet()
        {
            if (User.Identity.IsAuthenticated)
            {
                string userName = User.Claims.First(x => x.Type == ClaimTypes.Email).Value;
                CurrentUser = (from user in _context.Users
                               where user.UserName == userName
                               select user).SingleOrDefault();
                if (CurrentUser == null)
                {

                }
            }
        }

        public void ChangeUserDisplayName(string newName)
        {
            CurrentUser.DisplayName = newName;
            _context.Users.Where(x => x.Id == CurrentUser.Id).Single().DisplayName = newName;
            _context.SaveChanges();
        }
        
       

    }
}
