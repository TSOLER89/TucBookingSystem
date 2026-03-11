using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using TucBookingSystem.Api.DTOs;
using TucBookingSystem.Api.Services;
using TucBookingSystem.Shared.DTOs;

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
        return Ok(await _roomService.GetAllAsync());
    }

    [HttpGet("{id}")]
    public async Task<ActionResult<RoomDto>> GetById(int id)
    {
        var room = await _roomService.GetByIdAsync(id);
        if (room is null) return NotFound();

        return Ok(room);
    }

    [Authorize(Roles = "Admin")]
    [HttpPost]
    public async Task<ActionResult<RoomDto>> Create(CreateRoomDto dto)
    {
        var created = await _roomService.CreateAsync(dto);
        return CreatedAtAction(nameof(GetById), new { id = created.Id }, created);
    }
}