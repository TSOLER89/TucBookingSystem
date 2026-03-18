using FluentAssertions;
using Microsoft.Extensions.Logging;
using Moq;
using TucBookingSystem.Api.Models;
using TucBookingSystem.Api.Repositories;
using TucBookingSystem.Api.Services;
using TucBookingSystem.Shared.DTOs;

namespace TucBookingSystem.Tests;

public class RoomServiceTests
{
    private readonly Mock<IRoomRepository> _roomRepo;
    private readonly Mock<ILogger<RoomService>> _logger;
    private readonly RoomService _service;

    public RoomServiceTests()
    {
        _roomRepo = new Mock<IRoomRepository>();
        _logger = new Mock<ILogger<RoomService>>();
        _service = new RoomService(_roomRepo.Object, _logger.Object);
    }

    [Fact]
    public async Task GetAllAsync_ShouldReturnAllRooms()
    {
        _roomRepo.Setup(r => r.GetAllAsync()).ReturnsAsync(new List<Room>
        {
            new Room { Id = 1, Name = "Emmalund", Location = "Linköping", Capacity = 6 },
            new Room { Id = 2, Name = "Roxen", Location = "Linköping", Capacity = 10 }
        });

        var result = await _service.GetAllAsync();

        result.Should().HaveCount(2);
        result[0].Name.Should().Be("Emmalund");
        result[1].Name.Should().Be("Roxen");
    }

    [Fact]
    public async Task GetByIdAsync_ShouldReturnNull_WhenRoomNotFound()
    {
        _roomRepo.Setup(r => r.GetByIdAsync(99)).ReturnsAsync((Room?)null);

        var result = await _service.GetByIdAsync(99);

        result.Should().BeNull();
    }

    [Fact]
    public async Task GetByIdAsync_ShouldReturnRoom_WhenFound()
    {
        _roomRepo.Setup(r => r.GetByIdAsync(1)).ReturnsAsync(new Room
        {
            Id = 1,
            Name = "Emmalund",
            Location = "Linköping",
            Capacity = 6
        });

        var result = await _service.GetByIdAsync(1);

        result.Should().NotBeNull();
        result!.Id.Should().Be(1);
        result.Name.Should().Be("Emmalund");
    }

    [Fact]
    public async Task CreateAsync_ShouldReturnCreatedRoom()
    {
        _roomRepo.Setup(r => r.CreateAsync(It.IsAny<Room>()))
                 .ReturnsAsync((Room r) => { r.Id = 5; return r; });

        var dto = new CreateRoomDto
        {
            Name = "Stångån",
            Location = "Linköping",
            Capacity = 8,
            Description = "Stort konferensrum"
        };

        var result = await _service.CreateAsync(dto);

        result.Should().NotBeNull();
        result.Id.Should().Be(5);
        result.Name.Should().Be("Stångån");
        result.IsActive.Should().BeTrue();
    }
}
