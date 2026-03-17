using TucBookingSystem.Shared.DTOs;

namespace TucBookingSystem.Shared.Interfaces;

public interface IRoomService

{

    Task<List<RoomDto>> GetAllAsync();

    Task<RoomDto?> GetByIdAsync(int id);

    Task<RoomDto> CreateAsync(CreateRoomDto dto);

}
