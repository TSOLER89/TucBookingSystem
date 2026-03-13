using Microsoft.EntityFrameworkCore;
using TucBookingSystem.Api.Data;

using TucBookingSystem.Api.Models;

namespace TucBookingSystem.Api.Repositories;

public class BookingRepository : IBookingRepository
{
    private readonly ApplicationDbContext _context; 
    
    public BookingRepository(ApplicationDbContext context)
    {
        _context = context;
    }

    public async Task<List<Booking>> GetUserBookingsAsync(int userId)
    {
        return await _context.Bookings
            .Include(b => b.Room)
            .Where(b => b.UserId == userId)
            .ToListAsync();
    }

    public async Task<bool> HasConflictAsync(int roomId, DateOnly date, TimeOnly startTime, TimeOnly endTime)
    {
        return await _context.Bookings.AnyAsync(b =>
            b.RoomId == roomId &&
            b.Date == date &&
            startTime < b.EndTime &&
            endTime > b.StartTime);
    }

    public async Task<IEnumerable<Booking>> GetAllAsync()
    {
        return await _context.Bookings
            .Include(b => b.Room)
            .Include(b => b.User)
            .ToListAsync();
    }

    public async Task<Booking?> GetByIdAsync(int id)
    {
        return await _context.Bookings
            .Include(b => b.Room)
            .Include(b => b.User)
            .FirstOrDefaultAsync(b => b.Id == id);
    }

    public async Task<Booking> CreateAsync(Booking booking)
    {
        _context.Bookings.Add(booking);
        await _context.SaveChangesAsync();
        return booking;
    }

    public async Task<bool> DeleteAsync(int id)
    {
        var booking = await _context.Bookings.FindAsync(id);
        if (booking == null) return false;

        _context.Bookings.Remove(booking);
        await _context.SaveChangesAsync();
        return true;
    }
}