using GodlessBoard.Data;
using Microsoft.AspNetCore.SignalR;
using System.Security.Claims;

namespace GodlessBoard.Hubs
{
    public class GameHub : Hub
    {
        private MyDbContext _dbContext;
        public GameHub(MyDbContext myDbContext)
        {
            _dbContext = myDbContext;
        }  
        
        public async Task SendMessageAsync(int gameId, string username, string message)
        {
            if (Context.User == null || !Context.User.Identity.IsAuthenticated)
                return;
            await Clients.Group($"game{gameId}").SendAsync(username, message);
        }

        public async Task UpdateBoardAsync(int gameId)
        {
            if (Context.User == null || !Context.User.Identity.IsAuthenticated)
                return;
            Context.
            if (!Context.User.Claims.Any(x => x.Type == ClaimTypes.Email))
                return;
            var username = Context
                .User
                .Claims
                .Single(x => x.Type == ClaimTypes.Email)
                .Value;
            
            var currentUser = _dbContext.Users.SingleOrDefault(x => x.UserName == username);
            if (currentUser == null)
                return;
            
            if (!_dbContext.UserGameRole.Any(x => x.GameId == gameId && x.UserId == currentUser.Id))
                return;
            
            DateTime clientModified = DateTime.Now; // implement data acquiring from client 
            
            var game = _dbContext.Games.SingleOrDefault(x => x.Id == gameId);
            
            if (game == null || game.LastModified < clientModified)
                return;

            string gameJson = string.Empty;
            
            await Clients.Group($"game{gameId}").SendAsync(gameJson);
        }
    }
}
