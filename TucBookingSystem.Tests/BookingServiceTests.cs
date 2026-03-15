using TucBookingSystem.Api.Models;
using TucBookingSystem.Api.Repositories;
using TucBookingSystem.Api.Services;
using TucBookingSystem.Shared.DTOs;
using Xunit;

namespace TucBookingSystem.Tests;

public class BookingServiceTests
{
    [Fact]
    public async Task CreateAsync_ShouldFail_WhenStartTimeIsAfterEndTime()
    {
        var bookingRepo = new FakeBookingRepository();
        var roomRepo = new FakeRoomRepository();
        var service = new BookingService(bookingRepo, roomRepo);

        var dto = new CreateBookingDto
        {
            RoomId = 1,
            Date = DateOnly.FromDateTime(DateTime.Today.AddDays(1)),
            StartTime = new TimeOnly(14, 0),
            EndTime = new TimeOnly(13, 0),
            Purpose = "Test"
        };

        var result = await service.CreateAsync(1, dto);

        Assert.False(result.Success);
        Assert.Equal("Starttid måste vara före sluttid.", result.Message);
    }

    [Fact]
    public async Task CreateAsync_ShouldFail_WhenDateIsInThePast()
    {
        var bookingRepo = new FakeBookingRepository();
        var roomRepo = new FakeRoomRepository();
        var service = new BookingService(bookingRepo, roomRepo);

        var dto = new CreateBookingDto
        {
            RoomId = 1,
            Date = DateOnly.FromDateTime(DateTime.Today.AddDays(-1)),
            StartTime = new TimeOnly(10, 0),
            EndTime = new TimeOnly(11, 0),
            Purpose = "Test"
        };

        var result = await service.CreateAsync(1, dto);

        Assert.False(result.Success);
        Assert.Equal("Du kan inte boka ett datum i det förflutna.", result.Message);
    }
}

public class FakeBookingRepository : IBookingRepository
{
    public Task<List<Booking>> GetUserBookingsAsync(int userId)
        => Task.FromResult(new List<Booking>());

    public Task<bool> HasConflictAsync(int roomId, DateOnly date, TimeOnly startTime, TimeOnly endTime)
        => Task.FromResult(false);

    public Task<Booking> CreateAsync(Booking booking)
    {
        booking.Id = 1;
        return Task.FromResult(booking);
    }

    public Task<bool> DeleteAsync(int id)
        => Task.FromResult(true);

    public Task<IEnumerable<Booking>> GetAllAsync()
        => Task.FromResult<IEnumerable<Booking>>(new List<Booking>());

    public Task<Booking?> GetByIdAsync(int id)
        => Task.FromResult<Booking?>(null);
}

public class FakeRoomRepository : IRoomRepository
{
    public Task<List<Room>> GetAllAsync()
        => Task.FromResult(new List<Room>());

    public Task<Room?> GetByIdAsync(int id)
        => Task.FromResult<Room?>(new Room
        {
            Id = 1,
            Name = "Room A",
            Capacity = 4
        });

    public Task<Room> CreateAsync(Room room)
        => Task.FromResult(room);

    public Task<bool> UpdateAsync(int id, Room room)
        => Task.FromResult(true);

    public Task<bool> DeleteAsync(int id)
        => Task.FromResult(true);
}