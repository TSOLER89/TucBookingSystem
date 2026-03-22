using Microsoft.AspNetCore.Components.Authorization;
using System.Security.Claims;

namespace TucBookingSystem.Client.Services;

public class CustomAuthStateProvider : AuthenticationStateProvider
{
    private readonly IUserStateService _userState;

    public CustomAuthStateProvider(IUserStateService userState)
    {
        _userState = userState;
        _userState.OnChange += NotifyChanged;
    }

    public override async Task<AuthenticationState> GetAuthenticationStateAsync()
    {
        try
        {
            await _userState.LoadStateAsync();
        }
        catch
        {
            return new AuthenticationState(new ClaimsPrincipal(new ClaimsIdentity()));
        }

        if (!_userState.IsLoggedIn)
            return new AuthenticationState(new ClaimsPrincipal(new ClaimsIdentity()));

        var claims = new[]
        {
            new Claim(ClaimTypes.NameIdentifier, _userState.UserId.ToString()),
            new Claim(ClaimTypes.Name, _userState.FullName),
            new Claim(ClaimTypes.Email, _userState.Email),
            new Claim(ClaimTypes.Role, _userState.Role),
        };

        var identity = new ClaimsIdentity(claims, "jwt");
        return new AuthenticationState(new ClaimsPrincipal(identity));
    }

    private void NotifyChanged()
    {
        NotifyAuthenticationStateChanged(GetAuthenticationStateAsync());
    }
}
