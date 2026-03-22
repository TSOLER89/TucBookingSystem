using TucBookingSystem.Shared.DTOs;

namespace TucBookingSystem.Client.Services;

public interface IRoomService
{
    Task<List<RoomDto>> GetRoomsAsync();
    Task<RoomDto?> CreateRoomAsync(CreateRoomDto dto);
    Task<bool> DeleteRoomAsync(int id);

    Task<List<RoomDto>> GetAllAsync();
    Task<RoomDto?> GetByIdAsync(int id);
    Task<RoomDto> CreateAsync(CreateRoomDto dto);
    Task DeleteAsync(int id);
}