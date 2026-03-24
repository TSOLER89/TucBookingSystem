using FluentAssertions;
using Microsoft.EntityFrameworkCore;
using TucBookingSystem.Api.Data;
using TucBookingSystem.Api.Models;
using TucBookingSystem.Api.Repositories;

namespace TucBookingSystem.Tests;

public class BookingRepositoryIntegrationTests : IDisposable
{
    private readonly ApplicationDbContext _context;
    private readonly BookingRepository _repository;

    public BookingRepositoryIntegrationTests()
    {
        var options = new DbContextOptionsBuilder<ApplicationDbContext>()
            .UseInMemoryDatabase(databaseName: $"TestDb_{Guid.NewGuid()}")
            .Options;

        _context = new ApplicationDbContext(options);
        _repository = new BookingRepository(_context);
    }

    [Fact]
    public async Task GetAllAsync_ShouldReturnAllBookingsWithRelations()
    {
        // Arrange
        var room = new Room { Name = "Room 1", Location = "Loc", Capacity = 10 };
        var user = new User { FullName = "Test User", Email = "test@test.com", PasswordHash = "hash" };
        
        _context.Rooms.Add(room);
        _context.Users.Add(user);
        await _context.SaveChangesAsync();

        _context.Bookings.AddRange(
            new Booking
            {
                RoomId = room.Id,
                UserId = user.Id,
                Date = DateOnly.FromDateTime(DateTime.Today),
                StartTime = new TimeOnly(10, 0),
                EndTime = new TimeOnly(11, 0),
                Purpose = "Meeting 1"
            },
            new Booking
            {
                RoomId = room.Id,
                UserId = user.Id,
                Date = DateOnly.FromDateTime(DateTime.Today),
                StartTime = new TimeOnly(14, 0),
                EndTime = new TimeOnly(15, 0),
                Purpose = "Meeting 2"
            }
        );
        await _context.SaveChangesAsync();

        // Act
        var result = await _repository.GetAllAsync();

        // Assert
        result.Should().HaveCount(2);
        result.Should().OnlyContain(b => b.Room != null && b.User != null);
    }

    [Fact]
    public async Task GetByIdAsync_ShouldReturnBookingWithRelations()
    {
        // Arrange
        var room = new Room { Name = "Room", Location = "Loc", Capacity = 10 };
        var user = new User { FullName = "User", Email = "user@test.com", PasswordHash = "hash" };
        
        _context.Rooms.Add(room);
        _context.Users.Add(user);
        await _context.SaveChangesAsync();

        var booking = new Booking
        {
            RoomId = room.Id,
            UserId = user.Id,
            Date = DateOnly.FromDateTime(DateTime.Today),
            StartTime = new TimeOnly(10, 0),
            EndTime = new TimeOnly(11, 0),
            Purpose = "Test Meeting"
        };
        
        _context.Bookings.Add(booking);
        await _context.SaveChangesAsync();

        // Act
        var result = await _repository.GetByIdAsync(booking.Id);

        // Assert
        result.Should().NotBeNull();
        result!.Room.Should().NotBeNull();
        result.User.Should().NotBeNull();
        result.Purpose.Should().Be("Test Meeting");
    }

    [Fact]
    public async Task GetUserBookingsAsync_ShouldReturnUserBookings()
    {
        // Arrange
        var room = new Room { Name = "Room", Location = "Loc", Capacity = 10 };
        var user1 = new User { FullName = "User 1", Email = "user1@test.com", PasswordHash = "hash" };
        var user2 = new User { FullName = "User 2", Email = "user2@test.com", PasswordHash = "hash" };

        _context.Rooms.Add(room);
        _context.Users.AddRange(user1, user2);
        await _context.SaveChangesAsync();

        _context.Bookings.AddRange(
            new Booking { RoomId = room.Id, UserId = user1.Id, Date = DateOnly.FromDateTime(DateTime.Today), StartTime = new TimeOnly(10, 0), EndTime = new TimeOnly(11, 0), Purpose = "User1 Meeting1" },
            new Booking { RoomId = room.Id, UserId = user1.Id, Date = DateOnly.FromDateTime(DateTime.Today), StartTime = new TimeOnly(14, 0), EndTime = new TimeOnly(15, 0), Purpose = "User1 Meeting2" },
            new Booking { RoomId = room.Id, UserId = user2.Id, Date = DateOnly.FromDateTime(DateTime.Today), StartTime = new TimeOnly(16, 0), EndTime = new TimeOnly(17, 0), Purpose = "User2 Meeting" }
        );
        await _context.SaveChangesAsync();

        // Act
        var result = await _repository.GetUserBookingsAsync(user1.Id);

        // Assert
        result.Should().HaveCount(2);
        result.Should().OnlyContain(b => b.UserId == user1.Id);
    }

    [Fact]
    public async Task CreateAsync_ShouldAddBooking()
    {
        // Arrange
        var room = new Room { Name = "Room", Location = "Loc", Capacity = 10 };
        var user = new User { FullName = "User", Email = "user@test.com", PasswordHash = "hash" };
        
        _context.Rooms.Add(room);
        _context.Users.Add(user);
        await _context.SaveChangesAsync();

        var booking = new Booking
        {
            RoomId = room.Id,
            UserId = user.Id,
            Date = DateOnly.FromDateTime(DateTime.Today.AddDays(1)),
            StartTime = new TimeOnly(10, 0),
            EndTime = new TimeOnly(11, 0),
            Purpose = "New Meeting"
        };

        // Act
        var result = await _repository.CreateAsync(booking);

        // Assert
        result.Should().NotBeNull();
        result.Id.Should().BeGreaterThan(0);
        
        var savedBooking = await _context.Bookings.FindAsync(result.Id);
        savedBooking.Should().NotBeNull();
    }

    [Fact]
    public async Task DeleteAsync_ShouldRemoveBooking()
    {
        // Arrange
        var room = new Room { Name = "Room", Location = "Loc", Capacity = 10 };
        var user = new User { FullName = "User", Email = "user@test.com", PasswordHash = "hash" };
        
        _context.Rooms.Add(room);
        _context.Users.Add(user);
        await _context.SaveChangesAsync();

        var booking = new Booking
        {
            RoomId = room.Id,
            UserId = user.Id,
            Date = DateOnly.FromDateTime(DateTime.Today),
            StartTime = new TimeOnly(10, 0),
            EndTime = new TimeOnly(11, 0),
            Purpose = "To Delete"
        };
        
        _context.Bookings.Add(booking);
        await _context.SaveChangesAsync();
        var bookingId = booking.Id;

        // Act
        await _repository.DeleteAsync(bookingId);

        // Assert
        var deleted = await _context.Bookings.FindAsync(bookingId);
        deleted.Should().BeNull();
    }

    [Fact]
    public async Task HasConflictAsync_ShouldReturnTrue_WhenOverlap()
    {
        // Arrange
        var room = new Room { Name = "Room", Location = "Loc", Capacity = 10 };
        var user = new User { FullName = "User", Email = "user@test.com", PasswordHash = "hash" };
        
        _context.Rooms.Add(room);
        _context.Users.Add(user);
        await _context.SaveChangesAsync();

        var existingBooking = new Booking
        {
            RoomId = room.Id,
            UserId = user.Id,
            Date = DateOnly.FromDateTime(DateTime.Today),
            StartTime = new TimeOnly(10, 0),
            EndTime = new TimeOnly(12, 0),
            Purpose = "Existing"
        };
        
        _context.Bookings.Add(existingBooking);
        await _context.SaveChangesAsync();

        // Act - Try to book overlapping time
        var hasConflict = await _repository.HasConflictAsync(
            room.Id,
            DateOnly.FromDateTime(DateTime.Today),
            new TimeOnly(11, 0),
            new TimeOnly(13, 0)
        );

        // Assert
        hasConflict.Should().BeTrue();
    }

    [Fact]
    public async Task HasConflictAsync_ShouldReturnFalse_WhenNoOverlap()
    {
        // Arrange
        var room = new Room { Name = "Room", Location = "Loc", Capacity = 10 };
        var user = new User { FullName = "User", Email = "user@test.com", PasswordHash = "hash" };
        
        _context.Rooms.Add(room);
        _context.Users.Add(user);
        await _context.SaveChangesAsync();

        var existingBooking = new Booking
        {
            RoomId = room.Id,
            UserId = user.Id,
            Date = DateOnly.FromDateTime(DateTime.Today),
            StartTime = new TimeOnly(10, 0),
            EndTime = new TimeOnly(12, 0),
            Purpose = "Existing"
        };
        
        _context.Bookings.Add(existingBooking);
        await _context.SaveChangesAsync();

        // Act - Try to book non-overlapping time
        var hasConflict = await _repository.HasConflictAsync(
            room.Id,
            DateOnly.FromDateTime(DateTime.Today),
            new TimeOnly(13, 0),
            new TimeOnly(14, 0)
        );

        // Assert
        hasConflict.Should().BeFalse();
    }

    [Fact]
    public async Task HasConflictAsync_ShouldIgnoreExcludedBooking()
    {
        var room = new Room { Name = "Room", Location = "Loc", Capacity = 10 };
        var user = new User { FullName = "User", Email = "user@test.com", PasswordHash = "hash" };

        _context.Rooms.Add(room);
        _context.Users.Add(user);
        await _context.SaveChangesAsync();

        var existingBooking = new Booking
        {
            RoomId = room.Id,
            UserId = user.Id,
            Date = DateOnly.FromDateTime(DateTime.Today),
            StartTime = new TimeOnly(10, 0),
            EndTime = new TimeOnly(12, 0),
            Purpose = "Existing"
        };

        _context.Bookings.Add(existingBooking);
        await _context.SaveChangesAsync();

        var hasConflict = await _repository.HasConflictAsync(
            room.Id,
            existingBooking.Date,
            existingBooking.StartTime,
            existingBooking.EndTime,
            existingBooking.Id);

        hasConflict.Should().BeFalse();
    }

    public void Dispose()
    {
        _context.Database.EnsureDeleted();
        _context.Dispose();
    }
}
