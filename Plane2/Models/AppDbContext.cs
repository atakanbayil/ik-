using Microsoft.EntityFrameworkCore;
using Plane2.Models; // Replace with your actual namespace

public class AppDbContext : DbContext
{
    public AppDbContext(DbContextOptions<AppDbContext> options) : base(options)
    {
    }

    // DbSet for Users. Add other DbSets for additional entities as needed.
    public DbSet<User> Users { get; set; }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);

        // Model configurations go here
        // For example, configuring a User entity
        modelBuilder.Entity<User>(entity =>
        {
            entity.HasKey(e => e.Id);
            entity.Property(e => e.Email).IsRequired();
            // Add other property configurations as needed
        });
    }
}

public class User
{
    public int Id { get; set; }
    public string Name { get; set; } // Assuming these fields based on RegisterViewModel
    public string Surname { get; set; }
    public string Gender { get; set; }
    public string Email { get; set; }
    public string Password { get; set; }

    // Add other properties and relationships as needed
}
