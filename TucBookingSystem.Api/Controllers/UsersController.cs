using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using TucBookingSystem.Api.Repositories;
using TucBookingSystem.Shared.DTOs;

namespace TucBookingSystem.Api.Controllers;

[ApiController]
[Route("api/[controller]")]
[Authorize(Roles = "Admin")]
public class UsersController : ControllerBase
{
    private readonly IUserRepository _userRepository;

    public UsersController(IUserRepository userRepository)
    {
        _userRepository = userRepository;
    }

    [HttpGet]
    public async Task<ActionResult<List<UserDto>>> GetAll()
    {
        var users = await _userRepository.GetAllAsync();

        return Ok(users.Select(user => new UserDto
        {
            Id = user.Id,
            FullName = user.FullName,
            Email = user.Email,
            Role = user.Role
        }).ToList());
    }
}