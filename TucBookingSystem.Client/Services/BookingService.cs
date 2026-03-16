using System.Net.Http.Json;
using TucBookingSystem.Shared.DTOs;

namespace TucBookingSystem.Client.Services;

public class BookingService
{
    private readonly HttpClient _httpClient;

    public BookingService(HttpClient httpClient)
    {
        _httpClient = httpClient;
    }

    public async Task<List<BookingDto>> GetMyBookingsAsync(int userId)
    {
        var bookings = await _httpClient.GetFromJsonAsync<List<BookingDto>>($"api/bookings/my/{userId}");
        return bookings ?? new List<BookingDto>();
    }

    public async Task<(bool Success, string Message, BookingDto? Booking)> CreateBookingAsync(int userId, CreateBookingDto dto)
    {
        var response = await _httpClient.PostAsJsonAsync($"api/bookings/{userId}", dto);

        if (!response.IsSuccessStatusCode)
        {
            var error = await response.Content.ReadAsStringAsync();
            return (false, error, null);
        }

        var booking = await response.Content.ReadFromJsonAsync<BookingDto>();
        return (true, "Bokning skapad.", booking);
    }
}