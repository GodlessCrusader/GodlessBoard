using BlazorGameInstance.Model;
using GodlessBoard.Data;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace GodlessBoard.Pages
{
    public class IndexModel : PageModel
    {
        private readonly MyDbContext _dbContext;
        private readonly ILogger<IndexModel> _logger;
        public Game TestGenericGame { get; set; }
        public IndexModel(ILogger<IndexModel> logger, MyDbContext myDbContext)
        {
            _dbContext = myDbContext;
            _logger = logger;
        }

        public void OnGet()
        {
            TestGenericGame = Newtonsoft.Json.JsonConvert.DeserializeObject<Game>(_dbContext.Games.Where(x => x.Id == 3).Single().JsonRepresentation);
        }
    }
}