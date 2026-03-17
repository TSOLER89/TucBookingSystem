using Microsoft.AspNetCore.Mvc;
using TucBookingSystem.Api.Services;
using TucBookingSystem.Shared.DTOs;
using TucBookingSystem.Shared.Interfaces;

namespace TucBookingSystem.Api.Controllers;

[ApiController]
[Route("api/[controller]")]
public class RoomsController : ControllerBase
{
    private readonly IRoomService _roomService;

    public RoomsController(IRoomService roomService)
    {
        _roomService = roomService;
    }

    [HttpGet]
    public async Task<ActionResult<List<RoomDto>>> GetAll()
    {
        var rooms = await _roomService.GetAllAsync();
        return Ok(rooms);
    }

    [HttpGet("{id}")]
    public async Task<ActionResult<RoomDto>> GetById(int id)
    {
        var room = await _roomService.GetByIdAsync(id);

        if (room is null)
            return NotFound();

        return Ok(room);
    }

    [HttpPost]
    public async Task<ActionResult<RoomDto>> Create(CreateRoomDto dto)
    {
        var createdRoom = await _roomService.CreateAsync(dto);
        return CreatedAtAction(nameof(GetById), new { id = createdRoom.Id }, createdRoom);
    }

   
}