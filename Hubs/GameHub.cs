using GameModel;
using GodlessBoard.Data;
using GodlessBoard.Models;
using Microsoft.AspNetCore.SignalR;
using System.Security.Claims;
using System.Web.Http;

namespace GodlessBoard.Hubs
{
    [Authorize]
    public class GameHub : Hub
    {

        private MyDbContext _dbContext;

        public GameHub(MyDbContext myDbContext)
        {
            _dbContext = myDbContext;
        }  
        
        public Task<User> GetUserAsync()
        {
            if (Context.User == null || Context.User.Identity == null || !Context.User.Identity.IsAuthenticated)
                return null;
            
            //Context.Items[Context.Items.Count - 1] = gameId;
            
            if (!Context.User.Claims.Any(x => x.Type == ClaimTypes.Email))
                return null;
            
            var username = Context
                .User
                .Claims
                .Single(x => x.Type == ClaimTypes.Email)
                .Value;

            var currentUser = _dbContext.Users.SingleOrDefault(x => x.UserName == username);

            return Task.FromResult(currentUser);
        }

        public async Task HandleModificationRequestAsync(GameModel.Game.ModificationType type, IDictionary<string, string> args, int gameId)
        {

            var currentUser = await GetUserAsync();

            if (!_dbContext.UserGameRole.Any(x => x.GameId == gameId && x.UserId == currentUser.Id))
                return;

            var game = _dbContext.Games.SingleOrDefault(x => x.Id == gameId);

            if (game == null)
                return;

            switch(type)
            {
                case GameModel.Game.ModificationType.Board:
                    await UpdateBoardAsync(game, currentUser);
                    break;
                case GameModel.Game.ModificationType.Chat:
                   
                    break;
            }
                


        }

        public async Task RecieveMessageAsync(int gameId, TextMessage message)
        {
            var currentUser = await GetUserAsync();

            if (!_dbContext.UserGameRole.Any(x => x.GameId == gameId && x.UserId == currentUser.Id))
                return;

            var game = _dbContext.Games.SingleOrDefault(x => x.Id == gameId);

            if (game == null)
                return;
            var chat = new Chat();
            chat.Messages = new List<TextMessage>();
            var messages = _dbContext.Messages.Where(x => x.GameId == game.Id).ToList();

            Dictionary<int, string> userAvatars = new();

            var userIds = from m in _dbContext.UserGameRole
                          where m.GameId == game.Id
                          select m.Id;

            foreach (var id in userIds)
            {
                userAvatars.Add(id, (from m in _dbContext.Users
                                     where m.Id == id
                                     select m.ProfilePicUrl).Single());
            }

            foreach (var m in messages)
            {
                chat.Messages.Append(new TextMessage()
                {
                    Text = m.Text,
                    Id = m.Id,
                    RecievingTime = m.RecievingTime,
                    UserAvatarUrl = userAvatars[m.UserId],
                    UserId = m.UserId,
                    UserDisplayName = m.User.DisplayName
                });
            }
            var user = _dbContext.Users.Single(x => x.UserName == Context.User.Claims.Single(y => y.Type == ClaimTypes.Email).ToString());
            message.UserAvatarUrl = userAvatars[user.Id];
            message.RecievingTime = DateTime.Now;
            message.UserDisplayName = user.DisplayName;
            message.UserId = user.Id;
            chat.Messages.Append(message);
            await _dbContext.Messages.AddAsync(new ChatMessage()
            {
                GameId = gameId,
                RecievingTime = message.RecievingTime,
                UserId = user.Id
            });

            await _dbContext.SaveChangesAsync();
            await UpdateChatAsync(chat, currentUser, gameId);
        }
        public async Task JoinGameAsync(int gameId)
        {
            var currentUser = await GetUserAsync();

            if (!_dbContext.UserGameRole.Any(x => x.GameId == gameId && x.UserId == currentUser.Id))
                return;

            var game = _dbContext.Games.SingleOrDefault(x => x.Id == gameId);

            if (game == null)
                return;

            await Groups.AddToGroupAsync(Context.ConnectionId, $"game{gameId}");

        }

        public async Task LeaveGroupAsync(int gameId)
        {
            var currentUser = await GetUserAsync();

            if (!_dbContext.UserGameRole.Any(x => x.GameId == gameId && x.UserId == currentUser.Id))
                return;

            var game = _dbContext.Games.SingleOrDefault(x => x.Id == gameId);

            if (game == null)
                return;

            await Groups.RemoveFromGroupAsync(Context.ConnectionId, $"game{gameId}");

        }

        public async Task UpdateBoardAsync(Models.Game game, User user) 
        {
            await Clients.Group($"game{game.Id}").SendAsync("UpdateGameState", game);
        }

        public async Task UpdateChatAsync(Chat chat, User user, int gameId)
        {
            await Clients.Group($"game{gameId}").SendAsync("UpdateChatState", chat);
        }

    }
}
