using TucBookingSystem.Api.Models;
using TucBookingSystem.Api.Repositories;
using TucBookingSystem.Shared.DTOs;

namespace TucBookingSystem.Api.Services;

public class RoomService : IRoomService
{
    private readonly IRoomRepository _roomRepository;
    private readonly ILogger<RoomService> _logger;

    public RoomService(IRoomRepository roomRepository, ILogger<RoomService> logger)
    {
        _roomRepository = roomRepository;
        _logger = logger;
    }

    public async Task<List<RoomDto>> GetAllAsync()
    {
        _logger.LogInformation("Fetching all rooms");
        var rooms = await _roomRepository.GetAllAsync();
        _logger.LogInformation("Found {Count} rooms", rooms.Count);

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
        _logger.LogInformation("Fetching room {RoomId}", id);
        var room = await _roomRepository.GetByIdAsync(id);

        if (room is null)
        {
            _logger.LogWarning("Room {RoomId} not found", id);
            return null;
        }

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
        _logger.LogInformation("Creating new room: {RoomName}", dto.Name);

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
        _logger.LogInformation("Room {RoomId} created successfully", created.Id);

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

    public async Task<bool> DeleteAsync(int id)
    {
        _logger.LogInformation("Deleting room {RoomId}", id);
        var deleted = await _roomRepository.DeleteAsync(id);

        if (!deleted)
        {
            _logger.LogWarning("Room {RoomId} could not be deleted", id);
        }

        return deleted;
    }
}