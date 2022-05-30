using GodlessBoard.Data;
using GodlessBoard.Services;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.ModelBinding;
using Microsoft.AspNetCore.Mvc.RazorPages;
using System.ComponentModel.DataAnnotations;
using System.Security.Claims;

namespace GodlessBoard.Pages.Account
{
    public class RegisterModel : PageModel
    {
        private readonly GodlessBoard.Data.MyDbContext _context;
        [BindProperty]
        public Input Input { get; set; } = new Input();
        public void OnGet()
        {
        }

        public async Task<IActionResult> OnPostAsync()
        {
            if(Input.Password != Input.ConfirmPassword)
            {
                ModelState.AddModelError(string.Empty,"Password fields must match");
            }
            if (!ModelState.IsValid) return Page();

            var hg = new HashGenerator();
            hg.GenerateHash(Input.Password);

            var user = _context.Users.Where(x => x.UserName.ToUpper() == Input.UserName.ToUpper());
            if (user == null || !user.Any())
            {
                _context.Users.Add(new User()
                {
                    UserName = Input.UserName.ToUpper(),
                    DisplayName = Input.DisplayName,
                    PasswordHash = hg.PasswordHash,
                    PasswordSalt = hg.PasswordSalt
                });
                _context.SaveChanges();
                await Auth.Identify(HttpContext, Input.UserName, Input.DisplayName);
            }
            else
            {
                ModelState.AddModelError(string.Empty,"User with such email is already registered.");
                return Page();
            }
           
            return RedirectToPage("/Account/Index");
        }

        public RegisterModel(MyDbContext context)
        {
            _context = context;
        }
    }
    public class Input
    {
        [Required]
        [Display(Name = "Email")]
        public string UserName { get; set; }
        [Required]
        [Display(Name = "Display name")]
        public string DisplayName { get; set; }
        [Required]
        public string Password { get; set; }
        [Required, Display(Name = "Confirm Password")]
        public string ConfirmPassword { get; set; }
    }
}
