using TucBookingSystem.Api.Models;

namespace TucBookingSystem.Api.Services
{
    public interface IRoomService
    {
        Task<IEnumerable<Room>> GetAllRoomsAsync();

        Task<Room?> GetRoomByIdAsync(int id);

        Task<Room> CreateRoomAsync(Room room);

        Task<bool> UpdateRoomAsync(int id, Room room);

        Task<bool> DeleteRoomAsync(int id);
    }
}