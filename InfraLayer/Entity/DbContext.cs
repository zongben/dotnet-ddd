using InfraLayer.Entity.Models;
using Microsoft.EntityFrameworkCore;

namespace InfraLayer.Entity;

public class AppDbContext : DbContext
{
    public AppDbContext(DbContextOptions<AppDbContext> options)
        : base(options) { }

    public DbSet<UserEntity> Users => Set<UserEntity>();

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<UserEntity>(entity =>
        {
            entity.HasIndex(x => x.Account).IsUnique();
        });
    }
}
