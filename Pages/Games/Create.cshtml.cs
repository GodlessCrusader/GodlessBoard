using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.Mvc.Rendering;
using GodlessBoard.Models;
using GodlessBoard.Services;

namespace GodlessBoard.Pages.Games
{
    public class CreateModel : PageModel
    {
        private readonly GodlessBoard.Data.MyDbContext _context;

        public CreateModel(GodlessBoard.Data.MyDbContext context)
        {
            _context = context;
        }

        public IActionResult OnGet()
        {
            return Page();
        }

        [BindProperty]
        public Game Game { get; set; } = default!;
        

        // To protect from overposting attacks, see https://aka.ms/RazorPagesCRUD
        public async Task<IActionResult> OnPostAsync()
        {
            var owner = _context.Users.Where(u => u.UserName == Auth.GetUserName( User.Identity.Name)).Single();
            Game.Players.Add(owner);
          if (!ModelState.IsValid || Game == null)
            {
                return Page();
            }
          //if(_context.Games == null)
          //{
          //      _context.Games = new List<Game>();
          //}
            _context.Games.Add(Game);
            await _context.SaveChangesAsync();

            return RedirectToPage("./Index");
        }
    }
}
