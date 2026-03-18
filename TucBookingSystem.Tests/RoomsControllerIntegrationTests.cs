using System.Net;
using System.Net.Http.Json;
using FluentAssertions;
using TucBookingSystem.Shared.DTOs;

namespace TucBookingSystem.Tests;

public class RoomsControllerIntegrationTests : IClassFixture<TestWebApplicationFactory>
{
    private readonly HttpClient _client;

    public RoomsControllerIntegrationTests(TestWebApplicationFactory factory)
    {
        _client = factory.CreateClient();
    }

    [Fact]
    public async Task GetAll_ShouldReturnOk_AndListOfRooms()
    {
        // Act
        var response = await _client.GetAsync("/api/rooms");

        // Assert
        response.StatusCode.Should().Be(HttpStatusCode.OK);
        
        var rooms = await response.Content.ReadFromJsonAsync<List<RoomDto>>();
        rooms.Should().NotBeNull();
        rooms.Should().BeOfType<List<RoomDto>>();
    }

    [Fact]
    public async Task GetById_ShouldReturnNotFound_WhenRoomDoesNotExist()
    {
        // Act
        var response = await _client.GetAsync("/api/rooms/99999");

        // Assert
        response.StatusCode.Should().Be(HttpStatusCode.NotFound);
    }

    [Fact]
    public async Task Create_ShouldReturnUnauthorized_WhenNoToken()
    {
        // Arrange
        var newRoom = new CreateRoomDto
        {
            Name = "Test Room",
            Location = "Test Location",
            Capacity = 10,
            Description = "Test Description"
        };

        // Act
        var response = await _client.PostAsJsonAsync("/api/rooms", newRoom);

        // Assert
        response.StatusCode.Should().Be(HttpStatusCode.Unauthorized);
    }

    // Update not implemented in API, skipping test

    [Fact]
    public async Task Delete_ShouldReturnUnauthorized_WhenNoToken()
    {
        // Act
        var response = await _client.DeleteAsync("/api/rooms/1");

        // Assert
        response.StatusCode.Should().Be(HttpStatusCode.Unauthorized);
    }
}
