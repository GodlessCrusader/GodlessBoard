using GodlessBoard.Data;
using GodlessBoard.GameModel;
using GodlessBoard.Models;
using GodlessBoard.Services;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;

namespace GodlessBoard.Pages.Games
{
    public class PlayModel : PageModel
    {
        private readonly MyDbContext _dbContext;

        public User CurrentUser { get; private set; }

        public Models.Game CurrentGame { get; private set; }
        public List<ChatMessage> CurrentChat { get; set; }
        public PlayModel(MyDbContext myDbContext)
        {
            _dbContext = myDbContext;
        }
        
        public async Task OnGetAsync(int GameId)
        {
            if (User.Identity.IsAuthenticated) 
            {
                CurrentGame = _dbContext.Games.Where(x => x.Id == GameId).Single();
                CurrentUser = _dbContext.Users.Where(x => x.UserName == Auth.GetUserName(User.Identity.Name)).Single();
                CurrentChat = _dbContext.Messages.Where(x => x.Game.Id == CurrentGame.Id).ToList();
            }
            

            
        }
        
    }
}
