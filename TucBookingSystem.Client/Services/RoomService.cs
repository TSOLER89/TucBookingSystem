using System.Net.Http.Json;
using TucBookingSystem.Shared.DTOs;

namespace TucBookingSystem.Client.Services;

public class RoomService
{
    private readonly HttpClient _httpClient;

    public RoomService(HttpClient httpClient)
    {
        _httpClient = httpClient;
    }

    public async Task<List<RoomDto>> GetRoomsAsync()
    {
        var rooms = await _httpClient.GetFromJsonAsync<List<RoomDto>>("api/rooms");
        return rooms ?? new List<RoomDto>();
    }

    public async Task<RoomDto?> CreateRoomAsync(CreateRoomDto dto)
    {
        var response = await _httpClient.PostAsJsonAsync("api/rooms", dto);

        if (!response.IsSuccessStatusCode)
            return null;

        return await response.Content.ReadFromJsonAsync<RoomDto>();
    }
}