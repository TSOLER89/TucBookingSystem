using TucBookingSystem.Shared.DTOs;

namespace TucBookingSystem.Client.Services;

public interface INotificationService
{
    Task<List<NotificationDto>> GetMyNotificationsAsync();
    Task<int> GetUnreadCountAsync();
    Task MarkAsReadAsync(int id);
    Task MarkAllAsReadAsync();
}
