using Microsoft.EntityFrameworkCore;

namespace Infrastructure.Data;

public class HeroEfDbContext : DbContext
{
    public HeroEfDbContext(DbContextOptions options) : base(options) { }
}
