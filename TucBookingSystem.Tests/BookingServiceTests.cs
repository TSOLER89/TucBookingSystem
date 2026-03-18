using FluentAssertions;
using Microsoft.Extensions.Logging;
using Moq;
using TucBookingSystem.Api.Models;
using TucBookingSystem.Api.Repositories;
using TucBookingSystem.Api.Services;
using TucBookingSystem.Shared.DTOs;

namespace TucBookingSystem.Tests;

public class BookingServiceTests
{
    private readonly Mock<IBookingRepository> _bookingRepo;
    private readonly Mock<IRoomRepository> _roomRepo;
    private readonly Mock<ILogger<BookingService>> _logger;
    private readonly Mock<INotificationService> _notificationService;
    private readonly BookingService _service;

    public BookingServiceTests()
    {
        _bookingRepo = new Mock<IBookingRepository>();
        _roomRepo = new Mock<IRoomRepository>();
        _logger = new Mock<ILogger<BookingService>>();
        _notificationService = new Mock<INotificationService>();
        _notificationService.Setup(n => n.CreateAsync(It.IsAny<int>(), It.IsAny<string>())).Returns(Task.CompletedTask);
        _service = new BookingService(_bookingRepo.Object, _roomRepo.Object, _logger.Object, _notificationService.Object);
    }

    [Fact]
    public async Task CreateAsync_ShouldFail_WhenStartTimeIsAfterEndTime()
    {
        var dto = new CreateBookingDto
        {
            RoomId = 1,
            Date = DateOnly.FromDateTime(DateTime.Today.AddDays(1)),
            StartTime = new TimeOnly(14, 0),
            EndTime = new TimeOnly(13, 0),
            Purpose = "Test"
        };

        var result = await _service.CreateAsync(1, dto);

        result.Success.Should().BeFalse();
        result.Message.Should().Be("Starttid måste vara före sluttid.");
    }

    [Fact]
    public async Task CreateAsync_ShouldFail_WhenDateIsInThePast()
    {
        var dto = new CreateBookingDto
        {
            RoomId = 1,
            Date = DateOnly.FromDateTime(DateTime.Today.AddDays(-1)),
            StartTime = new TimeOnly(10, 0),
            EndTime = new TimeOnly(11, 0),
            Purpose = "Test"
        };

        var result = await _service.CreateAsync(1, dto);

        result.Success.Should().BeFalse();
        result.Message.Should().Be("Du kan inte boka ett datum i det förflutna.");
    }

    [Fact]
    public async Task CreateAsync_ShouldFail_WhenBookingOnSaturday()
    {
        // Hitta nästa lördag
        var today = DateTime.Today;
        var daysUntilSaturday = ((int)DayOfWeek.Saturday - (int)today.DayOfWeek + 7) % 7;
        if (daysUntilSaturday == 0) daysUntilSaturday = 7;
        var nextSaturday = today.AddDays(daysUntilSaturday);

        var dto = new CreateBookingDto
        {
            RoomId = 1,
            Date = DateOnly.FromDateTime(nextSaturday),
            StartTime = new TimeOnly(10, 0),
            EndTime = new TimeOnly(11, 0),
            Purpose = "Test"
        };

        var result = await _service.CreateAsync(1, dto);

        result.Success.Should().BeFalse();
        result.Message.Should().Be("Du kan inte boka rum på helger. Skolan är stängd lördagar och söndagar.");
    }

    [Fact]
    public async Task CreateAsync_ShouldFail_WhenBookingOnSunday()
    {
        // Hitta nästa söndag
        var today = DateTime.Today;
        var daysUntilSunday = ((int)DayOfWeek.Sunday - (int)today.DayOfWeek + 7) % 7;
        if (daysUntilSunday == 0) daysUntilSunday = 7;
        var nextSunday = today.AddDays(daysUntilSunday);

        var dto = new CreateBookingDto
        {
            RoomId = 1,
            Date = DateOnly.FromDateTime(nextSunday),
            StartTime = new TimeOnly(10, 0),
            EndTime = new TimeOnly(11, 0),
            Purpose = "Test"
        };

        var result = await _service.CreateAsync(1, dto);

        result.Success.Should().BeFalse();
        result.Message.Should().Be("Du kan inte boka rum på helger. Skolan är stängd lördagar och söndagar.");
    }

    [Fact]
    public async Task CreateAsync_ShouldFail_WhenStartTimeTooEarly()
    {
        var dto = new CreateBookingDto
        {
            RoomId = 1,
            Date = DateOnly.FromDateTime(DateTime.Today.AddDays(1)),
            StartTime = new TimeOnly(7, 0),
            EndTime = new TimeOnly(9, 0),
            Purpose = "Test"
        };

        var result = await _service.CreateAsync(1, dto);

        result.Success.Should().BeFalse();
        result.Message.Should().Be("Bokningar måste vara mellan 08:00 och 20:00.");
    }

    [Fact]
    public async Task CreateAsync_ShouldFail_WhenEndTimeTooLate()
    {
        var dto = new CreateBookingDto
        {
            RoomId = 1,
            Date = DateOnly.FromDateTime(DateTime.Today.AddDays(1)),
            StartTime = new TimeOnly(19, 0),
            EndTime = new TimeOnly(21, 0),
            Purpose = "Test"
        };

        var result = await _service.CreateAsync(1, dto);

        result.Success.Should().BeFalse();
        result.Message.Should().Be("Bokningar måste vara mellan 08:00 och 20:00.");
    }

    [Fact]
    public async Task CreateAsync_ShouldFail_WhenRoomNotFound()
    {
        _roomRepo.Setup(r => r.GetByIdAsync(99)).ReturnsAsync((Room?)null);

        var dto = new CreateBookingDto
        {
            RoomId = 99,
            Date = DateOnly.FromDateTime(DateTime.Today.AddDays(1)),
            StartTime = new TimeOnly(10, 0),
            EndTime = new TimeOnly(11, 0),
            Purpose = "Test"
        };

        var result = await _service.CreateAsync(1, dto);

        result.Success.Should().BeFalse();
        result.Message.Should().Be("Rummet finns inte.");
    }

    [Fact]
    public async Task CreateAsync_ShouldFail_WhenTimeConflictExists()
    {
        _roomRepo.Setup(r => r.GetByIdAsync(1)).ReturnsAsync(new Room { Id = 1, Name = "Room A" });
        _bookingRepo.Setup(r => r.HasConflictAsync(It.IsAny<int>(), It.IsAny<DateOnly>(), It.IsAny<TimeOnly>(), It.IsAny<TimeOnly>()))
                    .ReturnsAsync(true);

        var dto = new CreateBookingDto
        {
            RoomId = 1,
            Date = DateOnly.FromDateTime(DateTime.Today.AddDays(1)),
            StartTime = new TimeOnly(10, 0),
            EndTime = new TimeOnly(11, 0),
            Purpose = "Test"
        };

        var result = await _service.CreateAsync(1, dto);

        result.Success.Should().BeFalse();
        result.Message.Should().Be("Rummet är redan bokat den tiden.");
    }

    [Fact]
    public async Task CreateAsync_ShouldSucceed_WithValidData()
    {
        _roomRepo.Setup(r => r.GetByIdAsync(1)).ReturnsAsync(new Room { Id = 1, Name = "Room A" });
        _bookingRepo.Setup(r => r.HasConflictAsync(It.IsAny<int>(), It.IsAny<DateOnly>(), It.IsAny<TimeOnly>(), It.IsAny<TimeOnly>()))
                    .ReturnsAsync(false);
        _bookingRepo.Setup(r => r.CreateAsync(It.IsAny<Booking>()))
                    .ReturnsAsync((Booking b) => { b.Id = 1; return b; });

        var dto = new CreateBookingDto
        {
            RoomId = 1,
            Date = DateOnly.FromDateTime(DateTime.Today.AddDays(1)),
            StartTime = new TimeOnly(10, 0),
            EndTime = new TimeOnly(11, 0),
            Purpose = "Möte"
        };

        var result = await _service.CreateAsync(1, dto);

        result.Success.Should().BeTrue();
        result.Booking.Should().NotBeNull();
        result.Booking!.RoomId.Should().Be(1);
    }

    [Fact]
    public async Task DeleteAsync_ShouldFail_WhenBookingNotFound()
    {
        _bookingRepo.Setup(r => r.GetByIdAsync(999)).ReturnsAsync((Booking?)null);

        var result = await _service.DeleteAsync(999, 1);

        result.Success.Should().BeFalse();
        result.Message.Should().Be("Bokningen finns inte.");
    }

    [Fact]
    public async Task DeleteAsync_ShouldFail_WhenUserDoesNotOwnBooking()
    {
        _bookingRepo.Setup(r => r.GetByIdAsync(1)).ReturnsAsync(new Booking { Id = 1, UserId = 2 });

        var result = await _service.DeleteAsync(1, userId: 1);

        result.Success.Should().BeFalse();
        result.Message.Should().Be("Du får bara avboka dina egna bokningar.");
    }

    [Fact]
    public async Task DeleteAsync_ShouldSucceed_WhenUserOwnsBooking()
    {
        _bookingRepo.Setup(r => r.GetByIdAsync(1)).ReturnsAsync(new Booking { Id = 1, UserId = 1 });
        _bookingRepo.Setup(r => r.DeleteAsync(1)).ReturnsAsync(true);

        var result = await _service.DeleteAsync(1, userId: 1);

        result.Success.Should().BeTrue();
        result.Message.Should().Be("Bokningen avbokades.");
    }

    [Fact]
    public async Task GetAllAsync_ShouldReturnAllBookings()
    {
        _bookingRepo.Setup(r => r.GetAllAsync()).ReturnsAsync(new List<Booking>
        {
            new Booking { Id = 1, RoomId = 1, UserId = 1, Date = DateOnly.FromDateTime(DateTime.Today), StartTime = new TimeOnly(10, 0), EndTime = new TimeOnly(11, 0) },
            new Booking { Id = 2, RoomId = 2, UserId = 2, Date = DateOnly.FromDateTime(DateTime.Today), StartTime = new TimeOnly(12, 0), EndTime = new TimeOnly(13, 0) }
        });

        var result = await _service.GetAllBookings();

        result.Should().HaveCount(2);
    }

    [Fact]
    public async Task GetUserBookingsAsync_ShouldReturnOnlyUserBookings()
    {
        _bookingRepo.Setup(r => r.GetUserBookingsAsync(1)).ReturnsAsync(new List<Booking>
        {
            new Booking { Id = 1, RoomId = 1, UserId = 1, Date = DateOnly.FromDateTime(DateTime.Today), StartTime = new TimeOnly(10, 0), EndTime = new TimeOnly(11, 0) }
        });

        var result = await _service.GetUserBookingsAsync(1);

        result.Should().HaveCount(1);
        result[0].Id.Should().Be(1);
    }
}
