using DbService.Objects;
using Microsoft.EntityFrameworkCore;

namespace DbService
{
    public class AppDbContext : DbContext
    {
        public DbSet<StoreUser> Users { get; set; }
        public DbSet<StorePosition> Positions { get; set; }

        protected override void OnConfiguring(DbContextOptionsBuilder options)
        {
            options.UseSqlite("Data Source=app.db");
        }
    }
}