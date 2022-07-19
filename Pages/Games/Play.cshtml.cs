using GodlessBoard.Data;
using GodlessBoard.GameModel;
using GodlessBoard.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;

namespace GodlessBoard.Pages.Games
{
    public class PlayModel : PageModel
    {
        private readonly MyDbContext _dbContext;

        public Models.Game CurrentGame { get; private set; }

        public PlayModel(MyDbContext myDbContext)
        {
            _dbContext = myDbContext;
        }
        
        public async Task OnGetAsync(int GameId)
        {
            CurrentGame = _dbContext.Games.Where(x => x.Id == GameId).Single();
            if(CurrentGame.Chat == null)
            {
                var chat = new List<ChatMessage>();
                chat = _dbContext.Messages.Where(x => x.Game.Id == CurrentGame.Id).ToList();
                CurrentGame.Chat = chat;
            }
            
            
        }
        
    }
}
