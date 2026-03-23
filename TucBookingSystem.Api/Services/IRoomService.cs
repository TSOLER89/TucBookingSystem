using TucBookingSystem.Shared.DTOs;

namespace TucBookingSystem.Api.Services;

public interface IRoomService
{
    Task<List<RoomDto>> GetAllAsync();
    Task<RoomDto?> GetByIdAsync(int id);
    Task<RoomDto> CreateAsync(CreateRoomDto dto);
    Task<RoomDto?> UpdateAsync(int id, UpdateRoomDto dto);
    Task<bool> DeleteAsync(int id);
}
