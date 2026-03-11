using System.Collections.Generic;
using System.Threading.Tasks;

namespace TucBookingSystem.Api.Repositories;

//uppdaterat

public interface IRoomRepository
{
    Task<IEnumerable<Room>> GetAllAsync();
    Task<Room?> GetByIdAsync(int id);
    Task AddAsync(Room room);
    Task UpdateAsync(Room room);
    Task DeleteAsync(int id);
}