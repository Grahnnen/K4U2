using ContentAPI.Entities;
using Microsoft.EntityFrameworkCore;

namespace ContentAPI.Data
{
    public class AppDbContext : DbContext
    {
        public AppDbContext(DbContextOptions<AppDbContext> options) : base(options)
        {
        }

        public DbSet<AiContent> Contents => Set<AiContent>();
    }
}
