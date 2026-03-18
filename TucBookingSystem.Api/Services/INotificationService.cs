using TucBookingSystem.Shared.DTOs;

namespace TucBookingSystem.Api.Services;

public interface INotificationService
{
    Task<List<NotificationDto>> GetUserNotificationsAsync(int userId);
    Task CreateAsync(int userId, string message);
    Task<bool> MarkAsReadAsync(int id);
    Task<bool> MarkAllAsReadAsync(int userId);
    Task<int> GetUnreadCountAsync(int userId);
}
