using Microsoft.AspNetCore.Components.Server.ProtectedBrowserStorage;

namespace TucBookingSystem.Client.Services;

public class UserStateService
{
    private readonly ProtectedSessionStorage _sessionStorage;
    private const string USER_KEY = "currentUser";

    public event Action? OnChange;

    public bool IsLoggedIn { get; private set; }
    public string FullName { get; private set; } = string.Empty;
    public string Email { get; private set; } = string.Empty;
    public string Role { get; private set; } = string.Empty;
    public int UserId { get; private set; }

    public UserStateService(ProtectedSessionStorage sessionStorage)
    {
        _sessionStorage = sessionStorage;
    }

    public async Task LoadStateAsync()
    {
        try
        {
            var result = await _sessionStorage.GetAsync<UserData>(USER_KEY);

            if (result.Success && result.Value != null)
            {
                var data = result.Value;
                UserId = data.UserId;
                FullName = data.FullName;
                Email = data.Email;
                Role = data.Role;
                IsLoggedIn = true;
            }
        }
        catch (Exception ex)
        {
            Console.WriteLine($"LoadStateAsync: Error - {ex.Message}");
        }
    }

    public async Task SetUserAsync(int userId, string fullName, string email, string role)
    {
        UserId = userId;
        FullName = fullName;
        Email = email;
        Role = role;
        IsLoggedIn = true;

        var data = new UserData
        {
            UserId = userId,
            FullName = fullName,
            Email = email,
            Role = role
        };

        try
        {
            await _sessionStorage.SetAsync(USER_KEY, data);
        }
        catch (Exception ex)
        {
            Console.WriteLine($"SetUserAsync: Error - {ex.Message}");
        }

        NotifyStateChanged();
    }

    public async Task LogoutAsync()
    {
        UserId = 0;
        FullName = string.Empty;
        Email = string.Empty;
        Role = string.Empty;
        IsLoggedIn = false;

        await _sessionStorage.DeleteAsync(USER_KEY);
        NotifyStateChanged();
    }

    private void NotifyStateChanged() => OnChange?.Invoke();

    private class UserData
    {
        public int UserId { get; set; }
        public string FullName { get; set; } = string.Empty;
        public string Email { get; set; } = string.Empty;
        public string Role { get; set; } = string.Empty;
    }
}
