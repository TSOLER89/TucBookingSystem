using System.Collections.Generic;
using System.Threading.Tasks;

namespace TucBookingSystem.Api.Repositories;

public interface IBookingRepository
{
    Task<IEnumerable<Booking>> GetAllAsync();
    Task<Booking?> GetByIdAsync(int id);
    Task<IEnumerable<Booking>> GetByUserIdAsync(string userId);
    Task AddAsync(Booking booking);
    Task UpdateAsync(Booking booking);
    Task DeleteAsync(int id);
}