using FluentAssertions;
using Moq;
using System.Security.Claims;
using TucBookingSystem.Client.Services;

namespace TucBookingSystem.Tests;

public class AuthStateProviderTests
{
    private readonly Mock<IUserStateService> _userState;
    private readonly CustomAuthStateProvider _provider;

    public AuthStateProviderTests()
    {
        _userState = new Mock<IUserStateService>();
        _userState.Setup(u => u.LoadStateAsync()).Returns(Task.CompletedTask);
        _provider = new CustomAuthStateProvider(_userState.Object);
    }

    [Fact]
    public async Task GetAuthenticationStateAsync_ShouldReturnAnonymous_WhenNotLoggedIn()
    {
        _userState.Setup(u => u.IsLoggedIn).Returns(false);

        var state = await _provider.GetAuthenticationStateAsync();

        state.User.Identity!.IsAuthenticated.Should().BeFalse();
    }

    [Fact]
    public async Task GetAuthenticationStateAsync_ShouldReturnAuthenticated_WhenLoggedIn()
    {
        _userState.Setup(u => u.IsLoggedIn).Returns(true);
        _userState.Setup(u => u.FullName).Returns("Test User");
        _userState.Setup(u => u.Email).Returns("test@tuc.se");
        _userState.Setup(u => u.Role).Returns("User");
        _userState.Setup(u => u.UserId).Returns(1);

        var state = await _provider.GetAuthenticationStateAsync();

        state.User.Identity!.IsAuthenticated.Should().BeTrue();
    }

    [Fact]
    public async Task GetAuthenticationStateAsync_ShouldContainCorrectRole_WhenLoggedIn()
    {
        _userState.Setup(u => u.IsLoggedIn).Returns(true);
        _userState.Setup(u => u.FullName).Returns("Admin User");
        _userState.Setup(u => u.Email).Returns("admin@tuc.se");
        _userState.Setup(u => u.Role).Returns("Admin");
        _userState.Setup(u => u.UserId).Returns(1);

        var state = await _provider.GetAuthenticationStateAsync();

        state.User.IsInRole("Admin").Should().BeTrue();
    }

    [Fact]
    public async Task GetAuthenticationStateAsync_ShouldContainCorrectName_WhenLoggedIn()
    {
        _userState.Setup(u => u.IsLoggedIn).Returns(true);
        _userState.Setup(u => u.FullName).Returns("Jenny Svensson");
        _userState.Setup(u => u.Email).Returns("jenny@tuc.se");
        _userState.Setup(u => u.Role).Returns("User");
        _userState.Setup(u => u.UserId).Returns(2);

        var state = await _provider.GetAuthenticationStateAsync();

        state.User.FindFirst(ClaimTypes.Name)!.Value.Should().Be("Jenny Svensson");
    }

    [Fact]
    public async Task GetAuthenticationStateAsync_ShouldReturnAnonymous_WhenLoadStateThrows()
    {
        _userState.Setup(u => u.LoadStateAsync()).ThrowsAsync(new Exception("JS interop error"));

        var state = await _provider.GetAuthenticationStateAsync();

        state.User.Identity!.IsAuthenticated.Should().BeFalse();
    }
}
