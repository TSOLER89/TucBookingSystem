using TucBookingSystem.Api.DTOs;
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
            Description = r.Description
        }).ToList();
    }

    public async Task<RoomDto?> GetByIdAsync(int id)
    {
        var room = await _roomRepository.GetByIdAsync(id);

        if (room is null)
            return null;

        return new RoomDto
        {
            Id = room.Id,
            Name = room.Name,
            Location = room.Location,
            Capacity = room.Capacity,
            Description = room.Description
        };
    }

    public async Task<RoomDto> CreateAsync(CreateRoomDto dto)
    {
        var room = new Room
        {
            Name = dto.Name,
            Location = dto.Location,
            Capacity = dto.Capacity,
            Description = dto.Description
        };

        var createdRoom = await _roomRepository.AddAsync(room);

        return new RoomDto
        {
            Id = createdRoom.Id,
            Name = createdRoom.Name,
            Location = createdRoom.Location,
            Capacity = createdRoom.Capacity,
            Description = createdRoom.Description
        };
    }
}