using GodlessBoard.Services;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace GodlessBoard.Pages.Account
{
    public class LogoutModel : PageModel
    {
        private readonly Auth _auth;
        public LogoutModel(Auth auth)
        {
            _auth = auth;
        }
        public async Task<IActionResult> OnPostAsync()
        {
            await _auth.SignOutAsync(HttpContext);
            return RedirectToPage("/Index");
        }
    }
}
