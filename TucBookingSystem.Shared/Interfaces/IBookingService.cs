using TucBookingSystem.Shared.DTOs;

namespace TucBookingSystem.Shared.Interfaces;

public interface IBookingService
    {
        Task<List<BookingDto>> GetUserBookingsAsync(int userId);
        Task<(bool Success, string Message, BookingDto? Booking)> CreateAsync(int userId, CreateBookingDto dto);
        Task<(bool Success, string Message)> DeleteAsync(int bookingId, int userId);

        // Metodo per admin: restituisce tutte le prenotazioni
        Task<List<BookingDto>> GetAllBookings();
}
