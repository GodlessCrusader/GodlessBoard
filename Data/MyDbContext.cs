using GodlessBoard.Models;
using Microsoft.EntityFrameworkCore;
namespace GodlessBoard.Data
{
    public class MyDbContext : DbContext
    {
        public MyDbContext(DbContextOptions options) : base( options)
        {

        }
        public DbSet<User> Users { get; set; }
        public DbSet<Game> Games { get; set; }
        public DbSet<UserGameRole> UserGameRole { get; set; }

        protected override void OnModelCreating(ModelBuilder builder)
        {
            base.OnModelCreating(builder);
            // Customize the ASP.NET Identity model and override the defaults if needed.
            // For example, you can rename the ASP.NET Identity table names and more.
            // Add your customizations after calling base.OnModelCreating(builder);
        }
    }


}
