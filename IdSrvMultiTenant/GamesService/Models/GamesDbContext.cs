using Microsoft.EntityFrameworkCore;

namespace GamesService.Models
{
    public class GamesDbContext : DbContext
    {
        public DbSet<Game> Games { get; set; }

        public GamesDbContext(DbContextOptions<GamesDbContext> options) : base(options)
        {
            
        }
    }
}