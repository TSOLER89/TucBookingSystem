using Microsoft.AspNetCore.Components.Server.ProtectedBrowserStorage;

namespace TucBookingSystem.Client.Services;

public class UserStateService
{
    private readonly ProtectedSessionStorage _sessionStorage;
    private const string USER_KEY = "currentUser";
    private bool _isInitialized = false;

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
            Console.WriteLine("LoadStateAsync: Attempting to load from session storage...");
            var result = await _sessionStorage.GetAsync<UserData>(USER_KEY);
            Console.WriteLine($"LoadStateAsync: Success={result.Success}");

            if (result.Success && result.Value != null)
            {
                var data = result.Value;
                UserId = data.UserId;
                FullName = data.FullName;
                Email = data.Email;
                Role = data.Role;
                IsLoggedIn = true;
                Console.WriteLine($"LoadStateAsync: Loaded user {FullName}");
            }
            else
            {
                Console.WriteLine("LoadStateAsync: No data found in session storage");
            }
            _isInitialized = true;
        }
        catch (Exception ex)
        {
            Console.WriteLine($"LoadStateAsync: Error - {ex.Message}");
            _isInitialized = true;
        }
    }

    public async Task SetUserAsync(int userId, string fullName, string email, string role)
    {
        Console.WriteLine($"SetUserAsync: Saving user {fullName} to session storage...");

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
            Console.WriteLine("SetUserAsync: Successfully saved to session storage");
        }
        catch (Exception ex)
        {
            Console.WriteLine($"SetUserAsync: Error saving - {ex.Message}");
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
