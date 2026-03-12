namespace TucBookingSystem.Api.Data
{
    using global::TucBookingSystem.Api.Models;
    using Microsoft.EntityFrameworkCore;

    namespace TucBookingSystem.Api.Data
    {

        public class ApplicationDbContext : DbContext

        {

            public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)

                : base(options)

            {

            }

            public DbSet<ApplicationUser> Users => Set<ApplicationUser>();

            public DbSet<Room> Rooms => Set<Room>();

            public DbSet<Booking> Bookings => Set<Booking>();

            protected override void OnModelCreating(ModelBuilder modelBuilder)

            {

                base.OnModelCreating(modelBuilder);

                modelBuilder.Entity<ApplicationUser>()

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

            }

            public class ApplicationUser
            {
                internal object? Email;
            }
        }
    }

}
