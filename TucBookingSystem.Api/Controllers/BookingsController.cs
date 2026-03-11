using Microsoft.AspNetCore.Mvc;
using TucBookingSystem.Api.Services;
using TucBookingSystem.Shared.DTOs;

namespace TucBookingSystem.Api.Controllers;

[ApiController]
[Route("api/[controller]")]
public class BookingsController : ControllerBase
{
    private readonly IBookingService _bookingService;

    public BookingsController(IBookingService bookingService)
    {
        _bookingService = bookingService;
    }

    [HttpGet("my/{userId}")]
    public async Task<ActionResult<List<BookingDto>>> GetMyBookings(int userId)
    {
        var bookings = await _bookingService.GetUserBookingsAsync(userId);
        return Ok(bookings);
    }

    [HttpPost("{userId}")]
    public async Task<ActionResult> Create(int userId, CreateBookingDto dto)
    {
        var result = await _bookingService.CreateAsync(userId, dto);

        if (!result.Success)
            return BadRequest(result.Message);

        return Ok(result.Booking);
    }
}