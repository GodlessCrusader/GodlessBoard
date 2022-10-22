using GodlessBoard.Data;
using GodlessBoard.Models;

namespace GodlessBoard.Services
{
    public class JwtHandler
    {
        private readonly MyDbContext _dbContext;
   
        public JwtHandler( MyDbContext myDbContext)
        {
            _dbContext = myDbContext;
        }

        public async Task<User> GetUserAsync(string token)
        {

        }

        public async Task<ge>
    }
}
