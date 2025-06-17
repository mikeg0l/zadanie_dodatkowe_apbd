using Microsoft.EntityFrameworkCore;
using ZadanieDodatkowe.Models;

namespace ZadanieDodatkowe.Data;

public class AppDbContext : DbContext
{
    
    public DbSet<Event> Events { get; set; }
    public DbSet<Attendee> Attendees { get; set; }
    public DbSet<Speaker> Speakers { get; set; }
    
    public AppDbContext(DbContextOptions options) : base(options)
    {
    }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);

        var attendee = new Attendee
        {
            Id = 1,
            FirstName = "Boris",
            LastName = "Goryl",
            Email = "boris.goryl@example.com"
        };

        var speaker = new Speaker
        {
            Id = 1,
            FirstName = "Grzegorz",
            LastName = "Kurka"
        };
        modelBuilder.Entity<Attendee>().HasData(attendee);
        modelBuilder.Entity<Speaker>().HasData(speaker);
    }
}