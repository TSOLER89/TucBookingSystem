using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;
using TucBookingSystem.Api.Services;
using TucBookingSystem.Shared.DTOs;

namespace TucBookingSystem.Api.Controllers;

[ApiController]
[Route("api/[controller]")]
[Authorize]
public class NotificationsController : ControllerBase
{
    private readonly INotificationService _service;

    public NotificationsController(INotificationService service)
    {
        _service = service;
    }

    [HttpGet("my")]
    public async Task<ActionResult<List<NotificationDto>>> GetMyNotifications()
    {
        var userId = int.Parse(User.FindFirst(ClaimTypes.NameIdentifier)!.Value);
        var notifications = await _service.GetUserNotificationsAsync(userId);
        return Ok(notifications);
    }

    [HttpGet("unread-count")]
    public async Task<ActionResult<int>> GetUnreadCount()
    {
        var userId = int.Parse(User.FindFirst(ClaimTypes.NameIdentifier)!.Value);
        var count = await _service.GetUnreadCountAsync(userId);
        return Ok(count);
    }

    [HttpPut("{id}/read")]
    public async Task<ActionResult> MarkAsRead(int id)
    {
        var result = await _service.MarkAsReadAsync(id);
        if (!result) return NotFound();
        return Ok();
    }

    [HttpPut("mark-all-read")]
    public async Task<ActionResult> MarkAllAsRead()
    {
        var userId = int.Parse(User.FindFirst(ClaimTypes.NameIdentifier)!.Value);
        await _service.MarkAllAsReadAsync(userId);
        return Ok();
    }
}
