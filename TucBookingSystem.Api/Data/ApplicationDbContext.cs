using Microsoft.EntityFrameworkCore;
using TucBookingSystem.Api.Models;

namespace TucBookingSystem.Api.Data;

public class ApplicationDbContext : DbContext
{
    public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
        : base(options)
    {
    }

    public DbSet<User> Users => Set<User>();
    public DbSet<Room> Rooms => Set<Room>();
    public DbSet<Booking> Bookings => Set<Booking>();

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);

        modelBuilder.Entity<User>()
            .HasIndex(u => u.Email)
            .IsUnique();

        modelBuilder.Entity<Booking>()
            .HasOne(b => b.Room)
            .WithMany(r => r.Bookings)
            .HasForeignKey(b => b.RoomId);

        modelBuilder.Entity<Booking>()
            .HasOne(b => b.User)
            .WithMany(u => u.Bookings)
            .HasForeignKey(b => b.UserId);

        modelBuilder.Entity<Room>().HasData(
            new Room
            {
                Id = 1,
                Name = "Emmalund",
                Location = "Linköping",
                Capacity = 8,
                Description = "Mötesrum Emmalund",
                IsActive = true,
                CreatedAt = new DateTime(2026, 3, 16, 0, 0, 0, DateTimeKind.Utc)
            },
            new Room
            {
                Id = 2,
                Name = "Rosenkälla",
                Location = "Linköping",
                Capacity = 12,
                Description = "Mötesrum Rosenkälla",
                IsActive = true,
                CreatedAt = new DateTime(2026, 3, 16, 0, 0, 0, DateTimeKind.Utc)
            },
            new Room
            {
                Id = 3,
                Name = "Roxen",
                Location = "Linköping",
                Capacity = 4,
                Description = "Mötesrum Roxen",
                IsActive = true,
                CreatedAt = new DateTime(2026, 3, 16, 0, 0, 0, DateTimeKind.Utc)
            },
            new Room
            {
                Id = 4,
                Name = "Berg",
                Location = "Linköping",
                Capacity = 8,
                Description = "Mötesrum Berg",
                IsActive = true,
                CreatedAt = new DateTime(2026, 3, 16, 0, 0, 0, DateTimeKind.Utc)
            },
            new Room
            {
                Id = 5,
                Name = "Ådala",
                Location = "Linköping",
                Capacity = 6,
                Description = "Mötesrum Ådala",
                IsActive = true,
                CreatedAt = new DateTime(2026, 3, 16, 0, 0, 0, DateTimeKind.Utc)
            },
            new Room
            {
                Id = 6,
                Name = "Stångån",
                Location = "Linköping",
                Capacity = 6,
                Description = "Mötesrum Stångån",
                IsActive = true,
                CreatedAt = new DateTime(2026, 3, 16, 0, 0, 0, DateTimeKind.Utc)
            },
            new Room
            {
                Id = 7,
                Name = "Tinnerö",
                Location = "Linköping",
                Capacity = 4,
                Description = "Mötesrum Tinnerö",
                IsActive = true,
                CreatedAt = new DateTime(2026, 3, 16, 0, 0, 0, DateTimeKind.Utc)
            }
        );
    }
}