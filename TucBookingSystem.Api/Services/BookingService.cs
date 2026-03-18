using TucBookingSystem.Api.Models;
using TucBookingSystem.Api.Repositories;
using TucBookingSystem.Shared.DTOs;

namespace TucBookingSystem.Api.Services;

public class BookingService : IBookingService
{
    private readonly IBookingRepository _bookingRepository;
    private readonly IRoomRepository _roomRepository;
    private readonly ILogger<BookingService> _logger;
    private readonly INotificationService _notificationService;

    public BookingService(
        IBookingRepository bookingRepository,
        IRoomRepository roomRepository,
        ILogger<BookingService> logger,
        INotificationService notificationService)
    {
        _bookingRepository = bookingRepository;
        _roomRepository = roomRepository;
        _logger = logger;
        _notificationService = notificationService;
    }

    public async Task<List<BookingDto>> GetUserBookingsAsync(int userId)
    {
        var bookings = await _bookingRepository.GetUserBookingsAsync(userId);
        return bookings.Select(MapBookingDto).ToList();
    }

    public async Task<(bool Success, string Message, BookingDto? Booking)> CreateAsync(int userId, CreateBookingDto dto)
    {
        _logger.LogInformation("Creating booking for user {UserId}, room {RoomId}, date {Date}",
            userId, dto.RoomId, dto.Date);

        if (dto.StartTime >= dto.EndTime)
        {
            _logger.LogWarning("Booking creation failed: Start time after end time");
            return (false, "Starttid måste vara före sluttid.", null);
        }

        if (dto.Date < DateOnly.FromDateTime(DateTime.Today))
        {
            _logger.LogWarning("Booking creation failed: Date in the past");
            return (false, "Du kan inte boka ett datum i det förflutna.", null);
        }

        var dayOfWeek = dto.Date.DayOfWeek;
        if (dayOfWeek == DayOfWeek.Saturday || dayOfWeek == DayOfWeek.Sunday)
        {
            _logger.LogWarning("Booking creation failed: Weekend booking attempt for {Date}", dto.Date);
            return (false, "Du kan inte boka rum på helger. Skolan är stängd lördagar och söndagar.", null);
        }

        if (dto.StartTime < new TimeOnly(8, 0) || dto.EndTime > new TimeOnly(20, 0))
        {
            _logger.LogWarning("Booking creation failed: Time outside allowed hours");
            return (false, "Bokningar måste vara mellan 08:00 och 20:00.", null);
        }

        var room = await _roomRepository.GetByIdAsync(dto.RoomId);
        if (room is null)
        {
            _logger.LogWarning("Booking creation failed: Room {RoomId} not found", dto.RoomId);
            return (false, "Rummet finns inte.", null);
        }

        var hasConflict = await _bookingRepository.HasConflictAsync(dto.RoomId, dto.Date, dto.StartTime, dto.EndTime);
        if (hasConflict)
        {
            _logger.LogWarning("Booking creation failed: Time conflict for room {RoomId}", dto.RoomId);
            return (false, "Rummet är redan bokat den tiden.", null);
        }

        var booking = new Booking
        {
            RoomId = dto.RoomId,
            UserId = userId,
            Date = dto.Date,
            StartTime = dto.StartTime,
            EndTime = dto.EndTime,
            Purpose = dto.Purpose
        };

        var created = await _bookingRepository.CreateAsync(booking);
        _logger.LogInformation("Booking {BookingId} created successfully for user {UserId}",
            created.Id, userId);

        await _notificationService.CreateAsync(userId,
            $"Din bokning för {room.Name} den {dto.Date} ({dto.StartTime:HH\\:mm}–{dto.EndTime:HH\\:mm}) är bekräftad.");

        return (true, "Bokning skapad.", new BookingDto
        {
            Id = created.Id,
            RoomId = created.RoomId,
            RoomName = room.Name,
            Date = created.Date,
            StartTime = created.StartTime,
            EndTime = created.EndTime,
            Purpose = created.Purpose
        });
    }

    public async Task<(bool Success, string Message)> DeleteAsync(int bookingId, int userId)
    {
        _logger.LogInformation("Attempting to delete booking {BookingId} by user {UserId}", bookingId, userId);

        var booking = await _bookingRepository.GetByIdAsync(bookingId);

        if (booking is null)
        {
            _logger.LogWarning("Delete failed: Booking {BookingId} not found", bookingId);
            return (false, "Bokningen finns inte.");
        }

        if (booking.UserId != userId)
        {
            _logger.LogWarning("Delete failed: User {UserId} tried to delete booking {BookingId} owned by user {OwnerId}",
                userId, bookingId, booking.UserId);
            return (false, "Du får bara avboka dina egna bokningar.");
        }

        var deleted = await _bookingRepository.DeleteAsync(bookingId);

        if (!deleted)
        {
            _logger.LogError("Failed to delete booking {BookingId}", bookingId);
            return (false, "Kunde inte avboka bokningen.");
        }

        _logger.LogInformation("Booking {BookingId} deleted successfully", bookingId);

        await _notificationService.CreateAsync(userId,
            $"Din bokning (ID: {bookingId}) har avbokats.");

        return (true, "Bokningen avbokades.");
    }

    public async Task<List<BookingDto>> GetAllBookings()
    {
        var bookings = await _bookingRepository.GetAllAsync();
        return bookings.Select(MapBookingDto).ToList();
    }

    public async Task<List<BookingDto>> GetBookingsByRoomAndDateAsync(int roomId, DateOnly date)
    {
        _logger.LogInformation("Fetching bookings for room {RoomId} on date {Date}", roomId, date);

        var bookings = await _bookingRepository.GetAllAsync();
        var filteredBookings = bookings
            .Where(b => b.RoomId == roomId && b.Date == date)
            .OrderBy(b => b.StartTime)
            .Select(MapBookingDto)
            .ToList();

        _logger.LogInformation("Found {Count} bookings for room {RoomId} on {Date}",
            filteredBookings.Count, roomId, date);

        return filteredBookings;
    }

    private static BookingDto MapBookingDto(Booking booking)
    {
        return new BookingDto
        {
            Id = booking.Id,
            RoomId = booking.RoomId,
            RoomName = booking.Room?.Name ?? string.Empty,
            UserName = booking.User?.FullName ?? string.Empty,
            Date = booking.Date,
            StartTime = booking.StartTime,
            EndTime = booking.EndTime,
            Purpose = booking.Purpose
        };
    }
}
