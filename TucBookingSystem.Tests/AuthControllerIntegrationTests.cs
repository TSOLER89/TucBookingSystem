using System.Net;
using System.Net.Http.Json;
using FluentAssertions;
using TucBookingSystem.Shared.DTOs;

namespace TucBookingSystem.Tests;

public class AuthControllerIntegrationTests : IClassFixture<TestWebApplicationFactory>
{
    private readonly HttpClient _client;

    public AuthControllerIntegrationTests(TestWebApplicationFactory factory)
    {
        _client = factory.CreateClient();
    }

    [Fact]
    public async Task Register_ShouldReturnOk_WithNewUser()
    {
        // Arrange
        var registerDto = new RegisterRequestDto
        {
            FullName = "Test User",
            Email = $"test{Guid.NewGuid()}@test.com", // Unique email
            Password = "Password123!"
        };

        // Act
        var response = await _client.PostAsJsonAsync("/api/auth/register", registerDto);

        // Assert
        response.StatusCode.Should().Be(HttpStatusCode.OK);
        
        var result = await response.Content.ReadFromJsonAsync<UserDto>();
        result.Should().NotBeNull();
        result!.Email.Should().Be(registerDto.Email);
        result.FullName.Should().Be(registerDto.FullName);
    }

    [Fact]
    public async Task Register_ShouldReturnBadRequest_WhenEmailExists()
    {
        // Arrange
        var email = $"duplicate{Guid.NewGuid()}@test.com";
        
        var registerDto1 = new RegisterRequestDto
        {
            FullName = "User One",
            Email = email,
            Password = "Password123!"
        };

        // Register first user
        await _client.PostAsJsonAsync("/api/auth/register", registerDto1);

        // Try to register again with same email
        var registerDto2 = new RegisterRequestDto
        {
            FullName = "User Two",
            Email = email,
            Password = "DifferentPassword123!"
        };

        // Act
        var response = await _client.PostAsJsonAsync("/api/auth/register", registerDto2);

        // Assert
        response.StatusCode.Should().Be(HttpStatusCode.BadRequest);
    }

    [Fact]
    public async Task Login_ShouldReturnOk_WithValidCredentials()
    {
        // Arrange - First register a user
        var email = $"login{Guid.NewGuid()}@test.com";
        var password = "Password123!";
        
        var registerDto = new RegisterRequestDto
        {
            FullName = "Login Test User",
            Email = email,
            Password = password
        };

        await _client.PostAsJsonAsync("/api/auth/register", registerDto);

        // Act - Now try to login
        var loginDto = new LoginRequestDto
        {
            Email = email,
            Password = password
        };

        var response = await _client.PostAsJsonAsync("/api/auth/login", loginDto);

        // Assert
        response.StatusCode.Should().Be(HttpStatusCode.OK);

        var result = await response.Content.ReadFromJsonAsync<LoginResponseDto>();
        result.Should().NotBeNull();
        result!.Token.Should().NotBeNullOrEmpty();
        result.Email.Should().Be(email);
    }

    [Fact]
    public async Task Login_ShouldReturnUnauthorized_WithInvalidCredentials()
    {
        // Arrange
        var loginDto = new LoginRequestDto
        {
            Email = "nonexistent@test.com",
            Password = "WrongPassword123!"
        };

        // Act
        var response = await _client.PostAsJsonAsync("/api/auth/login", loginDto);

        // Assert
        response.StatusCode.Should().Be(HttpStatusCode.Unauthorized);
    }

    [Fact]
    public async Task RequestPasswordReset_ShouldReturnOk_ForAnyEmail()
    {
        // Arrange
        var email = "testreset@test.com";

        // Act
        var response = await _client.PostAsync($"/api/auth/request-password-reset?email={email}", null);

        // Assert
        response.StatusCode.Should().Be(HttpStatusCode.OK);
    }

    [Fact]
    public async Task ResetPassword_ShouldReturnBadRequest_WithInvalidToken()
    {
        // Arrange
        var resetDto = new ResetPasswordRequestDto
        {
            Email = "test@test.com",
            Token = "invalid-token",
            NewPassword = "NewPassword123!"
        };

        // Act
        var response = await _client.PostAsJsonAsync("/api/auth/reset-password", resetDto);

        // Assert
        response.StatusCode.Should().Be(HttpStatusCode.BadRequest);
    }
}
