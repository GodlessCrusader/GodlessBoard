using GodlessBoard.Data;
using GodlessBoard.Services;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using System.ComponentModel.DataAnnotations;

namespace GodlessBoard.Pages.Account
{
    public class LoginModel : PageModel
    {
        private readonly GodlessBoard.Data.MyDbContext _context;

        [BindProperty]
        public Credential Credential { get; set; }
        private readonly IConfiguration _configuration;
        public LoginModel(MyDbContext context,IConfiguration configuration)
        {
            _context = context;
            _configuration=configuration;
  
        }

        public void OnGet()
        {
        }
        public async Task<IActionResult> OnPostAsync()
        {
            if (!ModelState.IsValid) 
                return Page();
            var user = from users in _context.Users
                       where users.UserName == Credential.UserName
                       select users;
            if(user == null || user.Count()<1) 
                return BadRequest("There is no such Email-Password combination.");
            var hg = new HashGenerator();
            if (!hg.VerifyUserData(user.First(), Credential))
                return BadRequest("There is no such Email-Password combination.");
            else
            {
                await Auth.Identify(HttpContext, Credential.UserName, user.First().DisplayName);
                return RedirectToPage("/Index");
            }

        }
    }

    public class Credential
    {
        [Required]
        [Display(Name = "Login")]
        public string UserName { get; set; }
        [Required]
        [DataType(DataType.Password)]
        public string Password { get; set; }
    }
}
