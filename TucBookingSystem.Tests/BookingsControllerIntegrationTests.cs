using System.Net;
using System.Net.Http.Headers;
using System.Net.Http.Json;
using FluentAssertions;
using TucBookingSystem.Shared.DTOs;

namespace TucBookingSystem.Tests;

public class BookingsControllerIntegrationTests : IClassFixture<TestWebApplicationFactory>
{
    private readonly HttpClient _client;

    public BookingsControllerIntegrationTests(TestWebApplicationFactory factory)
    {
        _client = factory.CreateClient();
    }

    private async Task<string> GetAuthTokenAsync()
    {
        var email = $"bookingtest{Guid.NewGuid()}@test.com";
        var password = "Password123!";

        // Register user
        var registerDto = new RegisterRequestDto
        {
            FullName = "Booking Test User",
            Email = email,
            Password = password
        };

        await _client.PostAsJsonAsync("/api/auth/register", registerDto);

        // Login to get token
        var loginDto = new LoginRequestDto
        {
            Email = email,
            Password = password
        };

        var loginResponse = await _client.PostAsJsonAsync("/api/auth/login", loginDto);
        var loginResult = await loginResponse.Content.ReadFromJsonAsync<LoginResponseDto>();

        return loginResult!.Token;
    }

    [Fact]
    public async Task GetAll_ShouldReturnUnauthorized_WhenNoToken()
    {
        // Act
        var response = await _client.GetAsync("/api/bookings");

        // Assert
        response.StatusCode.Should().Be(HttpStatusCode.Unauthorized);
    }

    [Fact]
    public async Task GetAll_ShouldReturnOk_WithValidToken()
    {
        // Arrange
        var token = await GetAuthTokenAsync();
        _client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);

        // Act
        var response = await _client.GetAsync("/api/bookings");

        // Assert
        response.StatusCode.Should().Be(HttpStatusCode.OK);
        
        var bookings = await response.Content.ReadFromJsonAsync<List<BookingDto>>();
        bookings.Should().NotBeNull();
    }

    [Fact]
    public async Task GetById_ShouldReturnNotFound_WhenBookingDoesNotExist()
    {
        // Arrange
        var token = await GetAuthTokenAsync();
        _client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);

        // Act
        var response = await _client.GetAsync("/api/bookings/99999");

        // Assert
        response.StatusCode.Should().Be(HttpStatusCode.NotFound);
    }

    [Fact]
    public async Task Create_ShouldReturnUnauthorized_WhenNoToken()
    {
        // Arrange
        var bookingDto = new CreateBookingDto
        {
            RoomId = 1,
            Date = DateOnly.FromDateTime(DateTime.Today.AddDays(1)),
            StartTime = new TimeOnly(10, 0),
            EndTime = new TimeOnly(11, 0),
            Purpose = "Test Meeting"
        };

        // Act
        var response = await _client.PostAsJsonAsync("/api/bookings", bookingDto);

        // Assert
        response.StatusCode.Should().Be(HttpStatusCode.Unauthorized);
    }

    [Fact]
    public async Task Create_ShouldReturnBadRequest_WhenStartTimeAfterEndTime()
    {
        // Arrange
        var token = await GetAuthTokenAsync();
        _client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);

        var bookingDto = new CreateBookingDto
        {
            RoomId = 1,
            Date = DateOnly.FromDateTime(DateTime.Today.AddDays(1)),
            StartTime = new TimeOnly(14, 0),
            EndTime = new TimeOnly(13, 0), // End before start
            Purpose = "Invalid Meeting"
        };

        // Act
        var response = await _client.PostAsJsonAsync("/api/bookings", bookingDto);

        // Assert
        response.StatusCode.Should().Be(HttpStatusCode.BadRequest);
    }

    // Update not implemented in API, skipping test

    [Fact]
    public async Task Delete_ShouldReturnUnauthorized_WhenNoToken()
    {
        // Act
        var response = await _client.DeleteAsync("/api/bookings/1");

        // Assert
        response.StatusCode.Should().Be(HttpStatusCode.Unauthorized);
    }

    [Fact]
    public async Task Delete_ShouldReturnNotFound_WhenBookingDoesNotExist()
    {
        // Arrange
        var token = await GetAuthTokenAsync();
        _client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);

        // Act
        var response = await _client.DeleteAsync("/api/bookings/99999");

        // Assert
        response.StatusCode.Should().Be(HttpStatusCode.NotFound);
    }
}
