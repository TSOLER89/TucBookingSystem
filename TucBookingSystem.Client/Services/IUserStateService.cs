namespace TucBookingSystem.Client.Services;

public interface IUserStateService
{
    bool IsLoggedIn { get; }
    string FullName { get; }
    string Email { get; }
    string Role { get; }
    string Token { get; }
    int UserId { get; }
    bool IsAdmin { get; }

    event Action? OnChange;

    Task LoadStateAsync();
    Task SetUserAsync(int userId, string fullName, string email, string role, string token);
    Task LogoutAsync();
}
