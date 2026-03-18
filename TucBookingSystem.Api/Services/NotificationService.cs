using TucBookingSystem.Api.Models;
using TucBookingSystem.Api.Repositories;
using TucBookingSystem.Shared.DTOs;

namespace TucBookingSystem.Api.Services;

public class NotificationService : INotificationService
{
    private readonly INotificationRepository _repo;

    public NotificationService(INotificationRepository repo)
    {
        _repo = repo;
    }

    public async Task<List<NotificationDto>> GetUserNotificationsAsync(int userId)
    {
        var notifications = await _repo.GetByUserIdAsync(userId);

        return notifications.Select(n => new NotificationDto
        {
            Id = n.Id,
            Message = n.Message,
            IsRead = n.IsRead,
            CreatedAt = n.CreatedAt
        }).ToList();
    }

    public async Task CreateAsync(int userId, string message)
    {
        var notification = new Notification
        {
            UserId = userId,
            Message = message,
            IsRead = false,
            CreatedAt = DateTime.UtcNow
        };

        await _repo.CreateAsync(notification);
    }

    public async Task<bool> MarkAsReadAsync(int id)
    {
        return await _repo.MarkAsReadAsync(id);
    }

    public async Task<bool> MarkAllAsReadAsync(int userId)
    {
        return await _repo.MarkAllAsReadAsync(userId);
    }

    public async Task<int> GetUnreadCountAsync(int userId)
    {
        return await _repo.GetUnreadCountAsync(userId);
    }
}
