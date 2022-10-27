using GodlessBoard.Data;
using GodlessBoard.Models;
using GodlessBoard.Services;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.ModelBinding;
using Microsoft.AspNetCore.Mvc.RazorPages;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Security.Claims;

namespace GodlessBoard.Pages.Account
{
    public class RegisterModel : PageModel
    {
        private readonly Auth _auth;
        private readonly HashGenerator _hashGenerator;
        private readonly GodlessBoard.Data.MyDbContext _context;
        [BindProperty]
        public Input Input { get; set; } = new Input();
        public void OnGet()
        {
        }

        public async Task<IActionResult> OnPostAsync()
        {
            if (!ModelState.IsValid) return Page();

            

            var user = _context.Users.Where(x => x.UserName.ToUpper() == Input.UserName.ToUpper());
            if (user == null || !user.Any())
            {
                var hg = _hashGenerator;
                byte[] passwordHash;
                byte[] passwordSalt = null;

                _hashGenerator.GenerateHash(Input.Password, ref passwordSalt, out passwordHash);
                var newUser = new User()
                {
                    UserName = Input.UserName.ToUpper(),
                    DisplayName = Input.DisplayName,
                    PasswordHash = passwordHash,
                    PasswordSalt = passwordSalt,
                    ProfilePicUrl = "../image/defaultUserPic.png",
                    MaxMediaWeight = 9999999
                };
                _context.Users.Add(newUser);
                _context.SaveChanges();
                await _auth.IdentifyAsync(HttpContext, newUser);
            }
            else
            {
                ModelState.AddModelError(string.Empty,"User with such email is already registered.");
                return Page();
            }
           
            return RedirectToPage("/Account/Index");
        }

        public RegisterModel(MyDbContext context, HashGenerator hashGenerator)
        {
            _context = context;
            _hashGenerator = hashGenerator;
        }
    }
    public class Input
    {
        [Required]
        [EmailAddress]
        [Display(Name = "Email")]
        [StringLength(maximumLength: 254, MinimumLength = 5)]
        public string UserName { get; set; }
        [Required]
        [StringLength(maximumLength: 25, MinimumLength = 5)]
        [Display(Name = "Display name")]
        public string DisplayName { get; set; }
        [Required]
        [DataType(DataType.Password)]
        [PasswordPropertyText]
        
        [MinLength(8, ErrorMessage = "Password must contain at least 8 symbols.")]
        public string Password { get; set; }
        [Required, Display(Name = "Confirm Password")]
        [DataType(DataType.Password)]
        [Compare(nameof(Password), ErrorMessage ="Passwords mismatch")]
        public string ConfirmPassword { get; set; }
    }
}
