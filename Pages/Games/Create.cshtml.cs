using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using GodlessBoard.Models;
using GodlessBoard.Services;
using System.ComponentModel.DataAnnotations;
using GameModel;
using System.Security.Claims;

namespace GodlessBoard.Pages.Games
{
    public class CreateModel : PageModel
    {
        private readonly GodlessBoard.Data.MyDbContext _context;
        private readonly Auth _auth;
        public CreateModel(GodlessBoard.Data.MyDbContext context, Auth auth)
        {
            _context = context;
            _auth = auth;
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
            var userName = User.Claims.First(x => x.Type == ClaimTypes.Email).Value;
            var owner = _context.Users.Where(u => u.UserName == userName).Single();
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
                new GameModel.Game(game.Name,
                    new GameModel.Player(owner.DisplayName, Role.Owner)));
            
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
