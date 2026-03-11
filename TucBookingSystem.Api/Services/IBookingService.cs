using TucBookingSystem.Api.Models;

namespace TucBookingSystem.Api.Services
{
    public interface IBookingService
    {
        Task<IEnumerable<Booking>> GetAllBookingsAsync();

        Task<Booking?> GetBookingByIdAsync(int id);

        Task<Booking> CreateBookingAsync(Booking booking);

        Task<bool> DeleteBookingAsync(int id);
    }
}