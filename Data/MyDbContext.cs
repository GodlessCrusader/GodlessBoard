﻿using GodlessBoard.Models;
using Microsoft.EntityFrameworkCore;
namespace GodlessBoard.Data
{
    public class MyDbContext : DbContext
    {
        public MyDbContext(DbContextOptions options) : base(options)
        {

        }
        public DbSet<User> Users { get; set; }
        public DbSet<Game> Games { get; set; }
        public DbSet<UserGameRole> UserGameRole { get; set; }
        public DbSet<Media> Media { get; set; }
        public DbSet<ChatMessage> Messages { get; set; }

        protected override void OnModelCreating(ModelBuilder builder)
        {
            base.OnModelCreating(builder);
        }
    }


}
