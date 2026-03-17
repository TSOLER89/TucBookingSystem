using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using TucBookingSystem.Api.Data;
using TucBookingSystem.Api.Models;
using TucBookingSystem.Api.Services;
using TucBookingSystem.Shared.DTOs;

namespace TucBookingSystem.Api.Controllers;

[ApiController]
[Route("api/[controller]")]
public class AuthController : ControllerBase
{
    private readonly IAuthService _authService;
    private readonly ApplicationDbContext _context;

    public AuthController(IAuthService authService, ApplicationDbContext context)
    {
        _authService = authService;
        _context = context;
    }

    [HttpPost("register")]
    public async Task<ActionResult<UserDto>> Register(RegisterRequestDto dto)
    {
        if (!ModelState.IsValid)
            return BadRequest(ModelState);

        var result = await _authService.RegisterAsync(dto);

        if (result is null)
            return BadRequest("En användare med den e-postadressen finns redan.");

        return Ok(result);
    }

    [HttpPost("login")]
    public async Task<ActionResult<LoginResponseDto>> Login(LoginRequestDto dto)
    {
        var response = await _authService.LoginAsync(dto);

        if (response == null)
            return Unauthorized();

        return Ok(response);
    }
    [HttpPost("forgot-password")]
    [Consumes("application/json")]
    public async Task<IActionResult> ForgotPassword([FromBody] ForgotPasswordRequestDto request)
    {
        var user = await _context.Users.FirstOrDefaultAsync(u => u.Email == request.Email);
        if (user == null)
        {
            return Ok(new
            {
                message = "Om e-postadressen finns i systemet skickas en återställningslänk.",
                resetLink = ""  
            });
        }       

        var token = Guid.NewGuid().ToString();

        var resetToken = new PasswordResetToken
        {
            Email = request.Email,
            Token = token,
            ExpiresAt = DateTime.UtcNow.AddMinutes(30),
            IsUsed = false
        };

        _context.PasswordResetTokens.Add(resetToken);       
        await _context.SaveChangesAsync();

        var resetLink = $"https://localhost:7116/reset-password?token={token}&email={request.Email}";

        return Ok(new
        {
            message = "Återställningslänk skapad.",
            resetLink = resetLink
        });
    }

    [HttpPost("reset-password-api")]
    [Consumes("application/json")]
    public async Task<IActionResult> ResetPassword([FromBody] ResetPasswordRequestDto request)
    {
        var resetToken = await _context.PasswordResetTokens
            .FirstOrDefaultAsync(t =>
                t.Email == request.Email &&
                t.Token == request.Token &&
                !t.IsUsed &&
                t.ExpiresAt > DateTime.UtcNow);

        if (resetToken == null)
        {
            return BadRequest(new
            {
                message = "Ogiltig eller utgången token."
            });
        }

        var user = await _context.Users.FirstOrDefaultAsync(u => u.Email == request.Email);

        if (user == null)
        {
            return BadRequest(new
            {
                message = "Användaren hittades inte."
            });
        }

        var passwordHasher = new PasswordHasher<User>();
        user.PasswordHash = passwordHasher.HashPassword(user, request.NewPassword);

        resetToken.IsUsed = true;

        await _context.SaveChangesAsync();

        return Ok(new
        {
            message = "Lösenordet har uppdaterats."
        });
    }
}