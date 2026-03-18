using Microsoft.EntityFrameworkCore;
using TucBookingSystem.Api.Data;
using TucBookingSystem.Api.Models;

namespace TucBookingSystem.Api.Repositories;

public class UserRepository : IUserRepository
{
    private readonly ApplicationDbContext _context;

    public UserRepository(ApplicationDbContext context)
    {
        _context = context;
    }

    public async Task<User?> GetByEmailAsync(string email) =>
        await _context.Users.FirstOrDefaultAsync(u => u.Email == email);

    public async Task<User?> GetByIdAsync(int id) =>
        await _context.Users.FindAsync(id);

    public async Task<List<User>> GetAllAsync() =>
        await _context.Users
            .OrderBy(u => u.FullName)
            .ThenBy(u => u.Email)
            .ToListAsync();

    public async Task AddAsync(User user)
    {
        await _context.Users.AddAsync(user);
        await _context.SaveChangesAsync();
    }
}