using TucBookingSystem.Api.Models;
using TucBookingSystem.Api.Repositories;
using TucBookingSystem.Shared.DTOs;

namespace TucBookingSystem.Api.Services;

public class RoomService : IRoomService
{
    private readonly IRoomRepository _roomRepository;

    public RoomService(IRoomRepository roomRepository)
    {
        _roomRepository = roomRepository;
    }

    public async Task<List<RoomDto>> GetAllAsync()
    {
        var rooms = await _roomRepository.GetAllAsync();

        return rooms.Select(r => new RoomDto
        {
            Id = r.Id,
            Name = r.Name,
            Location = r.Location,
            Capacity = r.Capacity,
            Description = r.Description,
            IsActive = r.IsActive,
            CreatedAt = r.CreatedAt
        }).ToList();
    }

    public async Task<RoomDto?> GetByIdAsync(int id)
    {
        var room = await _roomRepository.GetByIdAsync(id);
        if (room is null) return null;

        return new RoomDto
        {
            Id = room.Id,
            Name = room.Name,
            Location = room.Location,
            Capacity = room.Capacity,
            Description = room.Description,
            IsActive = room.IsActive,
            CreatedAt = room.CreatedAt
        };
    }

    public async Task<RoomDto> CreateAsync(CreateRoomDto dto)
    {
        var room = new Room
        {
            Name = dto.Name,
            Location = dto.Location,
            Capacity = dto.Capacity,
            Description = dto.Description,
            IsActive = true,
            CreatedAt = DateTime.UtcNow
        };

        var created = await _roomRepository.CreateAsync(room);

        return new RoomDto
        {
            Id = created.Id,
            Name = created.Name,
            Location = created.Location,
            Capacity = created.Capacity,
            Description = created.Description,
            IsActive = created.IsActive,
            CreatedAt = created.CreatedAt
        };

    }
}