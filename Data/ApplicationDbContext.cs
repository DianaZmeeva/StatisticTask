using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using WebApplication1.Models;

namespace WebApplication1.Data
{
    public class ApplicationDbContext : IdentityDbContext<ApplicationUser>
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
            : base(options)
        {
        }

        public DbSet<KindOfSport> KindOfSports { get; set; }

        public DbSet<EventType> EventTypes { get; set; }

        public DbSet<Team> Teams { get; set; }

        public DbSet<Player> Players { get; set; }

        public DbSet<GameEvent> GameEvens { get; set; }

        public DbSet<GameProtocol> GameProtocols { get; set; }

        protected override void OnModelCreating(ModelBuilder builder)
        {
            base.OnModelCreating(builder);
            builder.Entity<GameProtocol>().HasOne(x => x.HomeTeam).WithMany().OnDelete(DeleteBehavior.Restrict);
            builder.Entity<GameProtocol>().HasOne(x => x.RivalTeam).WithMany().OnDelete(DeleteBehavior.Restrict);
        }
    }
}
