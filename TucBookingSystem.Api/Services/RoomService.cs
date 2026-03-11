using TucBookingSystem.Api.Models;
using TucBookingSystem.Api.Repositories;

namespace TucBookingSystem.Api.Services
{
    public class RoomService : IRoomService
    {
        private readonly IRoomRepository _roomRepository;

        public RoomService(IRoomRepository roomRepository)
        {
            _roomRepository = roomRepository;
        }

        public async Task<IEnumerable<Room>> GetAllRoomsAsync()
        {
            return await _roomRepository.GetAllAsync();
        }

        public async Task<Room?> GetRoomByIdAsync(int id)
        {
            return await _roomRepository.GetByIdAsync(id);
        }

        public async Task<Room> CreateRoomAsync(Room room)
        {
            return await _roomRepository.CreateAsync(room);
        }

        public async Task<bool> UpdateRoomAsync(int id, Room room)
        {
            return await _roomRepository.UpdateAsync(id, room);
        }

        public async Task<bool> DeleteRoomAsync(int id)
        {
            return await _roomRepository.DeleteAsync(id);
        }
    }
}