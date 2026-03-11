using TucBookingSystem.Api.DTOs;
using TucBookingSystem.Shared.DTOs;

namespace TucBookingSystem.Api.Services;

public interface IBookingService
{
    Task<List<BookingDto>> GetUserBookingsAsync(int userId);
    Task<(bool Success, string Message, BookingDto? Booking)> CreateAsync(int userId, CreateBookingDto dto);
}