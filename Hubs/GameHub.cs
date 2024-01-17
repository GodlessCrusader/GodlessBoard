using GameModel;
using GodlessBoard.Data;
using GodlessBoard.Models;
using GodlessBoard.Services;
using Microsoft.AspNetCore.SignalR;
using System.Security.Claims;

namespace GodlessBoard.Hubs
{
    
    public class GameHub : Hub
    {

        private MyDbContext _dbContext;
        private JwtHandler _jwtHandler;
        public GameHub(MyDbContext myDbContext, JwtHandler jwtHandler)
        {
            _dbContext = myDbContext;
            _jwtHandler = jwtHandler;
        }  
        
        public Task<User?> GetUserAsync()
        {
            User? currentUser = null;
            if (Context.User == null || Context.User.Identity == null || !Context.User.Identity.IsAuthenticated)
                return Task.FromResult(currentUser);
            
            //Context.Items[Context.Items.Count - 1] = gameId;
            
            if (!Context.User.Claims.Any(x => x.Type == ClaimTypes.Email))
                return Task.FromResult(currentUser);

            var username = Context
                .User
                .Claims
                .Single(x => x.Type == ClaimTypes.Email)
                .Value;

            currentUser = _dbContext.Users.SingleOrDefault(x => x.UserName == username);

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

        public async Task RecieveMessageAsync(int gameId, string authToken, TextMessage message)
        {
            
            var currentUser = _jwtHandler.GetUserByToken(authToken);

            if (!_dbContext.UserGameRole.Any(x => x.GameId == gameId && x.UserId == currentUser.Id))
                return;

            var game = _dbContext.Games.SingleOrDefault(x => x.Id == gameId);

            if (game == null)
                return;

            var chat = new Chat();
            chat.Messages = new List<TextMessage>();
            var messages = _dbContext.Messages.Where(x => x.GameId == game.Id).ToList();

            Dictionary<int, string> userAvatars = new();

            List<int> userIds = (from m in _dbContext.UserGameRole
                          where m.GameId == game.Id
                          select m.Id).ToList();

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
            //var user = _dbContext.Users.Single(x => x.UserName == currentUser.UserName);
            message.UserAvatarUrl = userAvatars[currentUser.Id];
            message.RecievingTime = DateTime.Now;
            message.UserDisplayName = currentUser.DisplayName;
            message.UserId = currentUser.Id;
            chat.Messages.Append(message);
            await _dbContext.Messages.AddAsync(new ChatMessage()
            {
                Text = message.Text,
                GameId = gameId,
                RecievingTime = message.RecievingTime,
                UserId = currentUser.Id
            });

            await _dbContext.SaveChangesAsync();
            await UpdateChatAsync(gameId);


        }
        public async Task JoinGameAsync(int gameId, string authToken)
        {
            var currentUser = _jwtHandler.GetUserByToken(authToken);

            if (!_dbContext.UserGameRole.Any(x => x.GameId == gameId && x.UserId == currentUser.Id))
                return;

            var game = _dbContext.Games.SingleOrDefault(x => x.Id == gameId);

            if (game == null)
                return;
            
            await Groups.AddToGroupAsync(Context.ConnectionId, $"game{gameId}");
            await UpdateChatAsync(gameId);
        }

       //

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

        public async Task UpdateChatAsync(int gameId)
        {
            var messages = _dbContext.Messages.Where(x => x.GameId == gameId).ToList();
            var textMessages = new List<TextMessage>();
            foreach (var message in messages)
            {
                var user = _dbContext.Users.Where(x => x.Id == message.UserId).Single();

                textMessages.Add(new TextMessage()
                {
                    Id = message.Id,
                    Text = message.Text,
                    UserAvatarUrl = user.ProfilePicUrl,
                    UserDisplayName = user.DisplayName,
                    UserId = message.UserId,
                    RecievingTime = message.RecievingTime
                });
            }
            var chat = new Chat()
            {
                Messages = textMessages
            };
            await Clients.Group($"game{gameId}").SendAsync("UpdateChatState", chat);
        }

        public async Task UpdateUserMediaAsync()
        {
            var user = await GetUserAsync();

            if (user != null) { await Clients.Caller.SendAsync("UpdateUserMedia", null); return; }

            var media = _dbContext.Media.Where(x => x.OwnerId == user.Id);

            await Clients.Caller.SendAsync("UpdateUserMedia", media);
        }

    }
}
