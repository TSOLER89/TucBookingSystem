using TucBookingSystem.Api.Models;

namespace TucBookingSystem.Api.Repositories;

public interface INotificationRepository
{
    Task<List<Notification>> GetByUserIdAsync(int userId);
    Task<Notification> CreateAsync(Notification notification);
    Task<bool> MarkAsReadAsync(int id);
    Task<bool> MarkAllAsReadAsync(int userId);
    Task<int> GetUnreadCountAsync(int userId);
}
