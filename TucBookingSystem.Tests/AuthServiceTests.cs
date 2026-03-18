using FluentAssertions;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using Moq;
using TucBookingSystem.Api.Models;
using TucBookingSystem.Api.Repositories;
using TucBookingSystem.Api.Services;
using TucBookingSystem.Shared.DTOs;

namespace TucBookingSystem.Tests;

public class AuthServiceTests
{
    private readonly Mock<IUserRepository> _userRepo;
    private readonly Mock<IConfiguration> _config;
    private readonly Mock<ILogger<AuthService>> _logger;
    private readonly AuthService _service;

    public AuthServiceTests()
    {
        _userRepo = new Mock<IUserRepository>();
        _config = new Mock<IConfiguration>();
        _logger = new Mock<ILogger<AuthService>>();

        _config.Setup(c => c["Jwt:Key"]).Returns("DettaArEnSuperHemligNyckelSomArMinst32Tecken");
        _config.Setup(c => c["Jwt:Issuer"]).Returns("TucBookingSystem");
        _config.Setup(c => c["Jwt:Audience"]).Returns("TucBookingSystemUsers");

        _service = new AuthService(_userRepo.Object, _config.Object, _logger.Object);
    }

    [Fact]
    public async Task RegisterAsync_ShouldReturnNull_WhenEmailAlreadyExists()
    {
        _userRepo.Setup(r => r.GetByEmailAsync("test@test.com"))
                 .ReturnsAsync(new User { Email = "test@test.com" });

        var dto = new RegisterRequestDto
        {
            FullName = "Test User",
            Email = "test@test.com",
            Password = "password123"
        };

        var result = await _service.RegisterAsync(dto);

        result.Should().BeNull();
    }

    [Fact]
    public async Task RegisterAsync_ShouldReturnUserDto_WhenNewUser()
    {
        _userRepo.Setup(r => r.GetByEmailAsync("new@test.com")).ReturnsAsync((User?)null);
        _userRepo.Setup(r => r.AddAsync(It.IsAny<User>())).Returns(Task.CompletedTask);

        var dto = new RegisterRequestDto
        {
            FullName = "New User",
            Email = "new@test.com",
            Password = "password123"
        };

        var result = await _service.RegisterAsync(dto);

        result.Should().NotBeNull();
        result!.Email.Should().Be("new@test.com");
        result.Role.Should().Be("User");
    }

    [Fact]
    public async Task LoginAsync_ShouldReturnNull_WhenUserNotFound()
    {
        _userRepo.Setup(r => r.GetByEmailAsync("notfound@test.com")).ReturnsAsync((User?)null);

        var dto = new LoginRequestDto
        {
            Email = "notfound@test.com",
            Password = "password123"
        };

        var result = await _service.LoginAsync(dto);

        result.Should().BeNull();
    }

    [Fact]
    public async Task LoginAsync_ShouldReturnNull_WhenPasswordIsWrong()
    {
        _userRepo.Setup(r => r.GetByEmailAsync("test@test.com"))
                 .ReturnsAsync(new User { Email = "test@test.com", PasswordHash = "correctpassword" });

        var dto = new LoginRequestDto
        {
            Email = "test@test.com",
            Password = "wrongpassword"
        };

        var result = await _service.LoginAsync(dto);

        result.Should().BeNull();
    }

    [Fact]
    public async Task LoginAsync_ShouldReturnToken_WhenCredentialsAreCorrect()
    {
        _userRepo.Setup(r => r.GetByEmailAsync("test@test.com"))
                 .ReturnsAsync(new User { Id = 1, FullName = "Test User", Email = "test@test.com", PasswordHash = "password123", Role = "User" });

        var dto = new LoginRequestDto
        {
            Email = "test@test.com",
            Password = "password123"
        };

        var result = await _service.LoginAsync(dto);

        result.Should().NotBeNull();
        result!.Token.Should().NotBeNullOrEmpty();
        result.Email.Should().Be("test@test.com");
    }
}
