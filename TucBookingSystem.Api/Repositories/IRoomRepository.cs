using TucBookingSystem.Api.Models;

namespace TucBookingSystem.Api.Repositories;

public interface IRoomRepository
{
    Task<List<Room>> GetAllAsync();
    Task<Room?> GetByIdAsync(int id);
    Task<Room> CreateAsync(Room room);
    Task<bool> UpdateAsync(int id, Room room);
    Task<bool> DeleteAsync(int id);
}