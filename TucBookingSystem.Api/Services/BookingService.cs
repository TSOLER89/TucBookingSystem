using TucBookingSystem.Api.DTOs;
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

        var createdBooking = await _bookingRepository.AddAsync(booking);

        return (true, "Bokning skapad.", new BookingDto
        {
            Id = createdBooking.Id,
            RoomId = createdBooking.RoomId,
            RoomName = room.Name,
            Date = createdBooking.Date,
            StartTime = createdBooking.StartTime,
            EndTime = createdBooking.EndTime,
            Purpose = createdBooking.Purpose
        });
    }
}