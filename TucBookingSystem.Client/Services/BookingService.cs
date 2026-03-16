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

    public async Task<List<BookingDto>> GetMyBookingsAsync()
    {
        var bookings = await _httpClient.GetFromJsonAsync<List<BookingDto>>("api/bookings/my");
        return bookings ?? new List<BookingDto>();
    }

    public async Task<(bool Success, string Message, BookingDto? Booking)> CreateBookingAsync(CreateBookingDto dto)
    {
        var response = await _httpClient.PostAsJsonAsync("api/bookings", dto);

        if (!response.IsSuccessStatusCode)
        {
            var error = await response.Content.ReadAsStringAsync();
            return (false, error, null);
        }

        var booking = await response.Content.ReadFromJsonAsync<BookingDto>();
        return (true, "Bokning skapad.", booking);
    }

    public async Task<(bool Success, string Message)> DeleteBookingAsync(int bookingId)
    {
        var response = await _httpClient.DeleteAsync($"api/bookings/{bookingId}");

        if (!response.IsSuccessStatusCode)
        {
            var error = await response.Content.ReadAsStringAsync();
            return (false, error);
        }

        var message = await response.Content.ReadAsStringAsync();
        return (true, message);
    }
}