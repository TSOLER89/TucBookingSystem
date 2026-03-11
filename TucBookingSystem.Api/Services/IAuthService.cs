using TucBookingSystem.Api.Models;

namespace TucBookingSystem.Api.Services
{
    public interface IAuthService
    {
        Task<string?> Login(string email, string password);
    }
}