using TucBookingSystem.Shared.DTOs;

namespace TucBookingSystem.Client.Services;

public interface IUserService
{
    Task<List<UserDto>> GetAllAsync();
}