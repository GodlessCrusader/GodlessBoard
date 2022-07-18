using GodlessBoard.Data;
using GodlessBoard.GameModel;
using GodlessBoard.Models;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace GodlessBoard.Pages
{
    public class IndexModel : PageModel
    {
        private readonly MyDbContext _dbContext;
        private readonly ILogger<IndexModel> _logger;
        public GameModel.Game TestGenericGame { get; set; }
        public Chat TestGenericChat { get; set; }
        public IndexModel(ILogger<IndexModel> logger, MyDbContext myDbContext)
        {
            _dbContext = myDbContext;
            _logger = logger;
        }

        public void OnGet()
        {
            var game = _dbContext.Games.Where(x => x.Id == 3).Single();
            TestGenericGame = Newtonsoft.Json.JsonConvert.DeserializeObject<GameModel.Game>(game.JsonRepresentation);
            TestGenericChat = game.Chat;
        }
    }
}