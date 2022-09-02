using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using GodlessBoard.Models;

namespace GodlessBoard.Pages.Games
{
    public class IndexModel : PageModel
    {
        public string ClientUrl = "https://localhost:7140/play/";
        private readonly GodlessBoard.Data.MyDbContext _context;

        public IndexModel(GodlessBoard.Data.MyDbContext context)
        {
            _context = context;
        }

        public IList<Game> Game { get;set; } = default!;

        public async Task OnGetAsync()
        {
            if (_context.Games != null)
            {
                Game = await _context.Games.ToListAsync();
            }
        }
    }
}
