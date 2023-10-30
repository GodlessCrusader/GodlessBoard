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
        public Credential Credential { get; set; } = new();
        private readonly IConfiguration _configuration;
        private readonly HashGenerator _hashGenerator;
        private readonly Auth _auth;
        public LoginModel(MyDbContext context,IConfiguration configuration, HashGenerator hashGenerator, Auth auth)
        {
            _context = context;
            _configuration = configuration;
            _hashGenerator = hashGenerator;
            _auth = auth;
        }

        public void OnGet()
        {
        }
        public async Task<IActionResult> OnPostAsync()
        {
            if (!ModelState.IsValid) 
                return Page();
            var user = from users in _context.Users
                       where users.UserName == Credential.UserName.ToUpper()
                       select users;
               
            if (user == null || user.Count() < 1 || !_hashGenerator.VerifyUserData(user.First(), Credential))
            {
                ModelState.AddModelError(string.Empty, "There is no such Email-Password combination.");
                return Page();
            }
            
            await _auth.SignInAsync(HttpContext, user.First());
            return RedirectToPage("/Index");

        }
    }

    public class Credential
    {
        [Required]
        [Display(Name = "Login")]
        public string UserName { get; set; } = string.Empty;
        [Required]
        [DataType(DataType.Password)]
        public string Password { get; set; } = string.Empty;
    }
}
