using Microsoft.AspNetCore.Identity;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using TucBookingSystem.Api.Models;
using TucBookingSystem.Api.Repositories;
using TucBookingSystem.Shared.DTOs;

namespace TucBookingSystem.Api.Services;

public class AuthService : IAuthService
{
    private readonly IUserRepository _userRepository;
    private readonly IConfiguration _configuration;
    private readonly ILogger<AuthService> _logger;

    public AuthService(
        IUserRepository userRepository, 
        IConfiguration configuration,
        ILogger<AuthService> logger)
    {
        _userRepository = userRepository;
        _configuration = configuration;
        _logger = logger;
    }

    public async Task<UserDto?> RegisterAsync(RegisterRequestDto dto)
    {
        _logger.LogInformation("Attempting to register user with email: {Email}", dto.Email);

        var existingUser = await _userRepository.GetByEmailAsync(dto.Email);

        if (existingUser is not null)
        {
            _logger.LogWarning("Registration failed: Email {Email} already exists", dto.Email);
            return null;
        }

        var user = new User
        {
            FullName = dto.FullName,
            Email = dto.Email,
            Role = "User"
        };

        var hasher = new PasswordHasher<User>();
        user.PasswordHash = hasher.HashPassword(user, dto.Password);

        await _userRepository.AddAsync(user);
        _logger.LogInformation("User registered successfully: {Email}", dto.Email);

        return new UserDto
        {
            Id = user.Id,
            FullName = user.FullName,
            Email = user.Email,
            Role = user.Role
        };
    }

    public async Task<LoginResponseDto?> LoginAsync(LoginRequestDto dto)
    {
        _logger.LogInformation("Login attempt for email: {Email}", dto.Email);

        var user = await _userRepository.GetByEmailAsync(dto.Email);

        if (user is null)
        {
            _logger.LogWarning("Login failed: User with email {Email} not found", dto.Email);
            return null;
        }

        var hasher = new PasswordHasher<User>();
        var result = hasher.VerifyHashedPassword(user, user.PasswordHash, dto.Password);

        if (result == PasswordVerificationResult.Failed)
        {
            _logger.LogWarning("Login failed: Invalid password for email {Email}", dto.Email);
            return null;
        }

        var token = GenerateJwtToken(user);
        _logger.LogInformation("User {Email} logged in successfully", dto.Email);

        return new LoginResponseDto
        {
            Token = token,
            UserId = user.Id,
            FullName = user.FullName,
            Email = user.Email,
            Role = user.Role
        };
    }

    private string GenerateJwtToken(User user)
    {
        var key = Encoding.UTF8.GetBytes(_configuration["Jwt:Key"]!);

        var claims = new List<Claim>
        {
            new Claim(ClaimTypes.NameIdentifier, user.Id.ToString()),
            new Claim(ClaimTypes.Name, user.FullName),
            new Claim(ClaimTypes.Email, user.Email),
            new Claim(ClaimTypes.Role, user.Role)
        };

        var token = new JwtSecurityToken(
            issuer: _configuration["Jwt:Issuer"],
            audience: _configuration["Jwt:Audience"],
            claims: claims,
            expires: DateTime.UtcNow.AddHours(2),
            signingCredentials: new SigningCredentials(
                new SymmetricSecurityKey(key),
                SecurityAlgorithms.HmacSha256)
        );

        return new JwtSecurityTokenHandler().WriteToken(token);
    }
}
