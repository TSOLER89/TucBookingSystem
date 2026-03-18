using System.Net.Http.Json;
using TucBookingSystem.Shared.DTOs;

namespace TucBookingSystem.Client.Services;

public class NotificationService : INotificationService
{
    private readonly HttpClient _httpClient;

    public NotificationService(HttpClient httpClient)
    {
        _httpClient = httpClient;
    }

    public async Task<List<NotificationDto>> GetMyNotificationsAsync()
    {
        try
        {
            var result = await _httpClient.GetFromJsonAsync<List<NotificationDto>>("api/notifications/my");
            return result ?? new List<NotificationDto>();
        }
        catch
        {
            return new List<NotificationDto>();
        }
    }

    public async Task<int> GetUnreadCountAsync()
    {
        try
        {
            return await _httpClient.GetFromJsonAsync<int>("api/notifications/unread-count");
        }
        catch
        {
            return 0;
        }
    }

    public async Task MarkAsReadAsync(int id)
    {
        try
        {
            await _httpClient.PutAsync($"api/notifications/{id}/read", null);
        }
        catch { }
    }

    public async Task MarkAllAsReadAsync()
    {
        try
        {
            await _httpClient.PutAsync("api/notifications/mark-all-read", null);
        }
        catch { }
    }
}
