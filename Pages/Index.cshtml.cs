using GodlessBoard.Data;
using GodlessBoard.GameModel;
using GodlessBoard.Models;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;

namespace GodlessBoard.Pages
{
    public class IndexModel : PageModel
    {
        private readonly MyDbContext _dbContext;
        private readonly ILogger<IndexModel> _logger;
        public GameModel.Game TestGenericGame { get; set; }
        
        public IndexModel(ILogger<IndexModel> logger, MyDbContext myDbContext)
        {
            _dbContext = myDbContext;
            _logger = logger;
        }

        public async Task OnGetAsync()
        {
            
        }
    }
}