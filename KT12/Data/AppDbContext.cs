using Microsoft.EntityFrameworkCore;
using КТ12.Models;

namespace КТ12.Data;

public class AppDbContext : DbContext
{
    public AppDbContext(DbContextOptions<AppDbContext> options) : base(options) { }

    public DbSet<User> Users => Set<User>();
    public DbSet<UserProfile> UserProfiles => Set<UserProfile>();

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);

        modelBuilder.Entity<User>(entity =>
        {
            entity.ToTable("Users");

            entity.HasIndex(u => u.UserName).IsUnique();

            entity.Property(u => u.CreatedAt).HasDefaultValueSql("CURRENT_TIMESTAMP");
        });

        modelBuilder.Entity<UserProfile>(entity =>
        {
            entity.ToTable("UserProfiles");

        });

        modelBuilder.Entity<User>()
            .HasOne(u => u.Profile)                       
            .WithOne(p => p.User)                        
            .HasForeignKey<UserProfile>(p => p.UserId)  
            .IsRequired()                                
            .OnDelete(DeleteBehavior.Cascade);            
    }
}
