using TucBookingSystem.Shared.DTOs;

namespace TucBookingSystem.Client.Services;

public interface IRoomService
{
    Task<List<RoomDto>> GetRoomsAsync();
    Task<RoomDto?> CreateRoomAsync(CreateRoomDto dto);

    Task<List<RoomDto>> GetAllAsync();
    Task<RoomDto?> GetByIdAsync(int id);
    Task<RoomDto> CreateAsync(CreateRoomDto dto);
}
