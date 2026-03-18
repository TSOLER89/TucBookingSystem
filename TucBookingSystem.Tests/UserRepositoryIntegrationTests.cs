using FluentAssertions;
using Microsoft.EntityFrameworkCore;
using TucBookingSystem.Api.Data;
using TucBookingSystem.Api.Models;
using TucBookingSystem.Api.Repositories;

namespace TucBookingSystem.Tests;

public class UserRepositoryIntegrationTests : IDisposable
{
    private readonly ApplicationDbContext _context;
    private readonly UserRepository _repository;

    public UserRepositoryIntegrationTests()
    {
        var options = new DbContextOptionsBuilder<ApplicationDbContext>()
            .UseInMemoryDatabase(databaseName: $"TestDb_{Guid.NewGuid()}")
            .Options;

        _context = new ApplicationDbContext(options);
        _repository = new UserRepository(_context);
    }

    [Fact]
    public async Task GetByIdAsync_ShouldReturnUser_WhenExists()
    {
        // Arrange
        var user = new User
        {
            FullName = "Test User",
            Email = "test@test.com",
            PasswordHash = "hashedpassword",
            Role = "User"
        };
        
        _context.Users.Add(user);
        await _context.SaveChangesAsync();

        // Act
        var result = await _repository.GetByIdAsync(user.Id);

        // Assert
        result.Should().NotBeNull();
        result!.FullName.Should().Be("Test User");
        result.Email.Should().Be("test@test.com");
    }

    [Fact]
    public async Task GetByIdAsync_ShouldReturnNull_WhenNotExists()
    {
        // Act
        var result = await _repository.GetByIdAsync(999);

        // Assert
        result.Should().BeNull();
    }

    [Fact]
    public async Task GetByEmailAsync_ShouldReturnUser_WhenExists()
    {
        // Arrange
        var user = new User
        {
            FullName = "Email Test",
            Email = "email@test.com",
            PasswordHash = "hash",
            Role = "User"
        };
        
        _context.Users.Add(user);
        await _context.SaveChangesAsync();

        // Act
        var result = await _repository.GetByEmailAsync("email@test.com");

        // Assert
        result.Should().NotBeNull();
        result!.Email.Should().Be("email@test.com");
        result.FullName.Should().Be("Email Test");
    }

    [Fact]
    public async Task GetByEmailAsync_ShouldReturnNull_WhenNotExists()
    {
        // Act
        var result = await _repository.GetByEmailAsync("nonexistent@test.com");

        // Assert
        result.Should().BeNull();
    }

    [Fact]
    public async Task AddAsync_ShouldAddUserToDatabase()
    {
        // Arrange
        var user = new User
        {
            FullName = "New User",
            Email = "newuser@test.com",
            PasswordHash = "newhash",
            Role = "User"
        };

        // Act
        await _repository.AddAsync(user);

        // Assert
        var savedUser = await _context.Users.FirstOrDefaultAsync(u => u.Email == "newuser@test.com");
        savedUser.Should().NotBeNull();
        savedUser!.FullName.Should().Be("New User");
    }

    public void Dispose()
    {
        _context.Database.EnsureDeleted();
        _context.Dispose();
    }
}
