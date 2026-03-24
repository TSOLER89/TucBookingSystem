using System.Net.Http.Json;
using TucBookingSystem.Shared.DTOs;

namespace TucBookingSystem.Client.Services;

public class BookingService : IBookingService
{
    private readonly HttpClient _httpClient;

    public BookingService(HttpClient httpClient)
    {
        _httpClient = httpClient;
    }

    public Task<List<BookingDto>> GetMyBookingsAsync()
    {
        return GetUserBookingsAsync(0);
    }

    public async Task<List<BookingDto>> GetUserBookingsAsync(int userId)
    {
        var bookings = await _httpClient.GetFromJsonAsync<List<BookingDto>>("api/bookings/my");
        return bookings ?? new List<BookingDto>();
    }

    public Task<(bool Success, string Message, BookingDto? Booking)> CreateBookingAsync(CreateBookingDto dto)
    {
        return CreateAsync(0, dto);
    }

    public Task<(bool Success, string Message, BookingDto? Booking)> UpdateBookingAsync(int bookingId, UpdateBookingDto dto)
    {
        return UpdateAsync(bookingId, 0, dto);
    }

    public async Task<(bool Success, string Message, BookingDto? Booking)> CreateAsync(int userId, CreateBookingDto dto)
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

    public async Task<(bool Success, string Message, BookingDto? Booking)> CreateForUserAsync(int userId, CreateBookingDto dto)
    {
        var response = await _httpClient.PostAsJsonAsync($"api/bookings/admin/{userId}", dto);

        if (!response.IsSuccessStatusCode)
        {
            var error = await response.Content.ReadAsStringAsync();
            return (false, error, null);
        }

        var booking = await response.Content.ReadFromJsonAsync<BookingDto>();
        return (true, "Bokning skapad.", booking);
    }

    public async Task<(bool Success, string Message, BookingDto? Booking)> UpdateAsync(int bookingId, int userId, UpdateBookingDto dto)
    {
        var response = await _httpClient.PutAsJsonAsync($"api/bookings/{bookingId}", dto);

        if (!response.IsSuccessStatusCode)
        {
            var error = await response.Content.ReadAsStringAsync();
            return (false, error, null);
        }

        var booking = await response.Content.ReadFromJsonAsync<BookingDto>();
        return (true, "Bokningen uppdaterades.", booking);
    }

    public Task<(bool Success, string Message)> DeleteBookingAsync(int bookingId)
    {
        return DeleteAsync(bookingId, 0);
    }

    public async Task<(bool Success, string Message)> DeleteAsync(int bookingId, int userId)
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

    public async Task<List<BookingDto>> GetAllBookings()
    {
        var bookings = await _httpClient.GetFromJsonAsync<List<BookingDto>>("api/bookings");
        return bookings ?? new List<BookingDto>();
    }

    public async Task<List<BookingDto>> GetBookingsByRoomAndDateAsync(int roomId, DateOnly date)
    {
        var dateString = date.ToString("yyyy-MM-dd");
        var bookings = await _httpClient.GetFromJsonAsync<List<BookingDto>>($"api/bookings/room/{roomId}/date/{dateString}");
        return bookings ?? new List<BookingDto>();
    }
}
