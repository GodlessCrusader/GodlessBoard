using GodlessBoard.Data;
using GodlessBoard.Services;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using System.ComponentModel.DataAnnotations;
using System.Security.Claims;

namespace GodlessBoard.Pages.Account
{
    public class RegisterModel : PageModel
    {
        private readonly GodlessBoard.Data.MyDbContext _context;
        [BindProperty]
        public Input Input { get; set; }
        public void OnGet()
        {
        }
        public async Task<IActionResult> OnPostAsync()
        {
            if (!ModelState.IsValid) return Page();

            var claims = new List<Claim>
            {
                new Claim(ClaimTypes.Email, Input.UserName)
            };

            var hg = new HashGenerator();
            hg.GenerateHash(Input.Password);
            var identity = new ClaimsIdentity(claims, "GodlessCookie");
            
            ClaimsPrincipal claimsPrincipal = new ClaimsPrincipal(identity);

            _context.Users.Add(new User()
            {
                UserName = Input.UserName,
                PasswordHash = hg.PasswordHash,
                PasswordSalt = hg.PasswordSalt
            });
            _context.SaveChanges();
            await HttpContext.SignInAsync("GodlessCookie", claimsPrincipal);

            return RedirectToPage("/Index");
        }

        public RegisterModel(MyDbContext context, Input input)
        {
            _context = context;
            Input = input;
        }
    }
    public class Input
    {
        [Required]
        [Display(Name = "Email")]
        public string UserName { get; set; }
        [Required]
        public string Password { get; set; }
        [Required, Display(Name = "Confirm Password")]
        public string ConfirmPassword { get; set; }
    }
}
