using System.Security.Claims;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using TucBookingSystem.Api.Services;
using TucBookingSystem.Shared.DTOs;

namespace TucBookingSystem.Api.Controllers;

[ApiController]
[Route("api/[controller]")]
[Authorize]
public class BookingsController : ControllerBase
{
    private readonly IBookingService _bookingService;

    public BookingsController(IBookingService bookingService)
    {
        _bookingService = bookingService;
    }

    [HttpGet("my")]
    public async Task<ActionResult<List<BookingDto>>> GetMyBookings()
    {
        var userIdClaim = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;

        if (!int.TryParse(userIdClaim, out var userId))
            return Unauthorized();

        var bookings = await _bookingService.GetUserBookingsAsync(userId);
        return Ok(bookings);
    }

    [HttpPost]
    public async Task<ActionResult> Create(CreateBookingDto dto)
    {
        var userIdClaim = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;

        if (!int.TryParse(userIdClaim, out var userId))
            return Unauthorized();

        var result = await _bookingService.CreateAsync(userId, dto);

        if (!result.Success)
            return BadRequest(result.Message);

        return Ok(result.Booking);
    }

    [HttpPost("admin/{userId:int}")]
    [Authorize(Roles = "Admin")]
    public async Task<ActionResult> CreateForUser(int userId, CreateBookingDto dto)
    {
        var result = await _bookingService.CreateAsync(userId, dto);

        if (!result.Success)
            return BadRequest(result.Message);

        return Ok(result.Booking);
    }

    [HttpDelete("{id}")]
    public async Task<ActionResult> Delete(int id)
    {
        var userIdClaim = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;

        if (!int.TryParse(userIdClaim, out var userId))
            return Unauthorized();

        var result = await _bookingService.DeleteAsync(id, userId);

        if (!result.Success)
            return BadRequest(result.Message);

        return Ok(result.Message);
    }

    [HttpGet]
    [Authorize(Roles = "Admin")]
    public async Task<ActionResult<List<BookingDto>>> GetAll()
    {
        var bookings = await _bookingService.GetAllBookings();
        return Ok(bookings);
    }

    [HttpGet("room/{roomId}/date/{date}")]
    [AllowAnonymous] // Alla kan se upptagna tider
    public async Task<ActionResult<List<BookingDto>>> GetBookingsByRoomAndDate(int roomId, string date)
    {
        if (!DateOnly.TryParse(date, out var parsedDate))
            return BadRequest("Ogiltigt datumformat. Använd YYYY-MM-DD.");

        var bookings = await _bookingService.GetBookingsByRoomAndDateAsync(roomId, parsedDate);
        return Ok(bookings);
    }
}