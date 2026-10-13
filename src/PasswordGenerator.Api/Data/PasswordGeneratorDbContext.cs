using Microsoft.EntityFrameworkCore;
using PasswordGenerator.Api.Models;

namespace PasswordGenerator.Api.Data;

public sealed class PasswordGeneratorDbContext : DbContext
{
    public PasswordGeneratorDbContext(DbContextOptions<PasswordGeneratorDbContext> options)
        : base(options)
    {
    }

    public DbSet<PasswordRecord> Passwords => Set<PasswordRecord>();

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<PasswordRecord>(entity =>
        {
            entity.HasKey(password => password.Id);
            entity.Property(password => password.Password)
                .IsRequired()
                .HasMaxLength(1024);

            entity.Property(password => password.CreatedAt)
                .IsRequired();
        });

        base.OnModelCreating(modelBuilder);
    }
}
