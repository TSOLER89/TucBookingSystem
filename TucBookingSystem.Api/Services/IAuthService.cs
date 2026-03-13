using TucBookingSystem.Shared.DTOs;

namespace TucBookingSystem.Api.Services;

public interface IAuthService
{
    Task<UserDto?> RegisterAsync(RegisterRequestDto dto);
    Task<LoginResponseDto?> LoginAsync(LoginRequestDto dto);
}