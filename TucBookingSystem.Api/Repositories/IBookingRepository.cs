using TucBookingSystem.Api.Models;

namespace TucBookingSystem.Api.Repositories;

public interface IBookingRepository
{
    Task<List<Booking>> GetUserBookingsAsync(int userId);
    Task<bool> HasConflictAsync(int roomId, DateOnly date, TimeOnly startTime, TimeOnly endTime);
    Task<Booking> CreateAsync(Booking booking);
    Task<bool> DeleteAsync(int id);
    Task<IEnumerable<Booking>> GetAllAsync();
    Task<Booking?> GetByIdAsync(int id);
    Task<bool> UpdateAsync(int id, Booking booking);
}