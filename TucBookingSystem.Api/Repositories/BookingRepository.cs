using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Threading.Tasks;
using TucBookingSystem.Api.Data;
using TucBookingSystem.Api.Models;

namespace TucBookingSystem.Api.Repositories;

public class BookingRepository : IBookingRepository
{
    private readonly AppDbContext _context;

    public BookingRepository(AppDbContext context)
    {
        _context = context;
    }

    public async Task<IEnumerable<Booking>> GetAllAsync() =>
        await _context.Bookings.Include(b => b.User).Include(b => b.Room).ToListAsync();

    public async Task<Booking?> GetByIdAsync(int id) =>
        await _context.Bookings.Include(b => b.User).Include(b => b.Room).FirstOrDefaultAsync(b => b.Id == id);

    public async Task<IEnumerable<Booking>> GetByUserIdAsync(string userId) =>
        await _context.Bookings.Where(b => b.UserId == userId).Include(b => b.Room).ToListAsync();

    public async Task AddAsync(Booking booking)
    {
        await _context.Bookings.AddAsync(booking);
        await _context.SaveChangesAsync();
    }

    public async Task UpdateAsync(Booking booking)
    {
        _context.Bookings.Update(booking);
        await _context.SaveChangesAsync();
    }

    public async Task DeleteAsync(int id)
    {
        var booking = await _context.Bookings.FindAsync(id);
        if (booking != null)
        {
            _context.Bookings.Remove(booking);
            await _context.SaveChangesAsync();
        }
    }
}