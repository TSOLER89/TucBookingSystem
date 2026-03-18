using TucBookingSystem.Shared.DTOs;

namespace TucBookingSystem.Client.Services;

public interface IBookingService
{
    Task<List<BookingDto>> GetMyBookingsAsync();
    Task<(bool Success, string Message, BookingDto? Booking)> CreateBookingAsync(CreateBookingDto dto);
    Task<(bool Success, string Message, BookingDto? Booking)> CreateForUserAsync(int userId, CreateBookingDto dto);
    Task<(bool Success, string Message)> DeleteBookingAsync(int bookingId);
    Task<List<BookingDto>> GetAllBookings();

    // Temporary admin-compatible methods kept to avoid changing page behavior.
    Task<List<BookingDto>> GetUserBookingsAsync(int userId);
    Task<(bool Success, string Message, BookingDto? Booking)> CreateAsync(int userId, CreateBookingDto dto);
    Task<(bool Success, string Message)> DeleteAsync(int bookingId, int userId);
}