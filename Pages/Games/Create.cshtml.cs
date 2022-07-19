using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.Mvc.Rendering;
using GodlessBoard.Models;
using GodlessBoard.Services;
using System.ComponentModel.DataAnnotations;
using GodlessBoard.GameModel;

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
        public Input Game { get; set; } = default!;
        

        // To protect from overposting attacks, see https://aka.ms/RazorPagesCRUD
        public async Task<IActionResult> OnPostAsync()
        {
            var owner = _context.Users.Where(u => u.UserName == Auth.GetUserName( User.Identity.Name)).Single();
            var game = new GodlessBoard.Models.Game();
            game.Players = new List<User>();
            game.Players.Add(owner);
            
          if (!ModelState.IsValid || Game == null)
            {
                return Page();
            }
            //if(_context.Games == null)
            //{
            //      _context.Games = new List<Game>();
            //}
            game.Name = Game.Name;
            game.Bio = Game.Bio;
            game.JsonRepresentation = Newtonsoft.Json.JsonConvert.SerializeObject(
                new GodlessBoard.GameModel.Game(game.Name,
                    new GodlessBoard.GameModel.Player(owner.DisplayName, Role.Owner)));
            
            _context.Games.Add(game);
            
            _context.UserGameRole.Add(new UserGameRole()
            {
                User = owner,
                Game = game,
                Role = Roles.Owner
            });
            
            await _context.SaveChangesAsync();

            return RedirectToPage("./Index");
        }
    }
    public class Input
    {
        [Required(ErrorMessage ="This field is required")]
        public string Name { get; set; }
        public string Bio { get; set; }

        
    }
}
