using System.Net.Http.Json;
using TucBookingSystem.Shared.DTOs;

namespace TucBookingSystem.Client.Services;

public class RoomService : IRoomService
{
    private readonly HttpClient _httpClient;

    public RoomService(HttpClient httpClient)
    {
        _httpClient = httpClient;
    }

    public Task<List<RoomDto>> GetRoomsAsync()
    {
        return GetAllAsync();
    }

    public async Task<List<RoomDto>> GetAllAsync()
    {
        var rooms = await _httpClient.GetFromJsonAsync<List<RoomDto>>("api/rooms");
        return rooms ?? new List<RoomDto>();
    }

    public async Task<RoomDto?> GetByIdAsync(int id)
    {
        return await _httpClient.GetFromJsonAsync<RoomDto>($"api/rooms/{id}");
    }

    public async Task<RoomDto?> CreateRoomAsync(CreateRoomDto dto)
    {
        try
        {
            return await CreateAsync(dto);
        }
        catch
        {
            return null;
        }
    }

    public async Task<RoomDto> CreateAsync(CreateRoomDto dto)
    {
        var response = await _httpClient.PostAsJsonAsync("api/rooms", dto);

        if (!response.IsSuccessStatusCode)
            throw new HttpRequestException($"Kunde inte skapa rum. Statuskod: {response.StatusCode}");

        return await response.Content.ReadFromJsonAsync<RoomDto>()
            ?? throw new InvalidOperationException("API returnerade inget rum.");
    }
}