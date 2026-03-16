using TucBookingSystem.Api.Models;
using TucBookingSystem.Api.Repositories;
using TucBookingSystem.Shared.DTOs;

namespace TucBookingSystem.Api.Services;

public class BookingService : IBookingService
{
    private readonly IBookingRepository _bookingRepository;
    private readonly IRoomRepository _roomRepository;

    public BookingService(IBookingRepository bookingRepository, IRoomRepository roomRepository)
    {
        _bookingRepository = bookingRepository;
        _roomRepository = roomRepository;
    }

    public async Task<List<BookingDto>> GetUserBookingsAsync(int userId)
    {
        var bookings = await _bookingRepository.GetUserBookingsAsync(userId);

        return bookings.Select(b => new BookingDto
        {
            Id = b.Id,
            RoomId = b.RoomId,
            RoomName = b.Room?.Name ?? string.Empty,
            Date = b.Date,
            StartTime = b.StartTime,
            EndTime = b.EndTime,
            Purpose = b.Purpose
        }).ToList();
    }

    public async Task<(bool Success, string Message, BookingDto? Booking)> CreateAsync(int userId, CreateBookingDto dto)
    {
        if (dto.StartTime >= dto.EndTime)
            return (false, "Starttid måste vara före sluttid.", null);

        if (dto.Date < DateOnly.FromDateTime(DateTime.Today))
            return (false, "Du kan inte boka ett datum i det förflutna.", null);

        if (dto.StartTime < new TimeOnly(8, 0) || dto.EndTime > new TimeOnly(20, 0))
            return (false, "Bokningar måste vara mellan 08:00 och 20:00.", null);

        var room = await _roomRepository.GetByIdAsync(dto.RoomId);
        if (room is null)
            return (false, "Rummet finns inte.", null);

        var hasConflict = await _bookingRepository.HasConflictAsync(dto.RoomId, dto.Date, dto.StartTime, dto.EndTime);
        if (hasConflict)
            return (false, "Rummet är redan bokat den tiden.", null);

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
        var booking = await _bookingRepository.GetByIdAsync(bookingId);

        if (booking is null)
            return (false, "Bokningen finns inte.");

        if (booking.UserId != userId)
            return (false, "Du får bara avboka dina egna bokningar.");

        var deleted = await _bookingRepository.DeleteAsync(bookingId);

        if (!deleted)
            return (false, "Kunde inte avboka bokningen.");

        return (true, "Bokningen avbokades.");
    }

    public async Task<List<BookingDto>> GetAllAsync()
    {
        var bookings = await _bookingRepository.GetAllAsync();

        return bookings.Select(b => new BookingDto
        {
            Id = b.Id,
            RoomId = b.RoomId,
            RoomName = b.Room?.Name ?? string.Empty,
            Date = b.Date,
            StartTime = b.StartTime,
            EndTime = b.EndTime,
            Purpose = b.Purpose
        }).ToList();
    }
}