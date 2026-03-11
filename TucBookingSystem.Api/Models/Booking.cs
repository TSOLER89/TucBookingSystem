using System.ComponentModel.DataAnnotations;

namespace TucBookingSystem.Api.Models;

public class Booking
{
    public int Id { get; set; }

    [Required]
    public int RoomId { get; set; }

    public Room? Room { get; set; }

    [Required]
    public int UserId { get; set; }

    public User? User { get; set; }

    [Required]
    public DateOnly Date { get; set; }

    [Required]
    public TimeOnly StartTime { get; set; }

    [Required]
    public TimeOnly EndTime { get; set; }

    [MaxLength(200)]
    public string Purpose { get; set; } = string.Empty;
}