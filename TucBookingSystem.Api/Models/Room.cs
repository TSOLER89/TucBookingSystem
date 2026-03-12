using System.ComponentModel.DataAnnotations;

namespace TucBookingSystem.Api.Models;

public class Room
{
    public int Id { get; set; }

    [Required]
    [MaxLength(100)]
    public string Name { get; set; } = string.Empty;

    [Required]
    [MaxLength(100)]
    public string Location { get; set; } = string.Empty;

    [Range(1, 1000)]
    public int Capacity { get; set; }

    [MaxLength(300)]
    public string Description { get; set; } = string.Empty;

    public ICollection<Booking> Bookings { get; set; } = new List<Booking>();

    //tillgängligt för bokning
    //internal set förhindrar att egenskapen sätts utanför assembly:n.
    public bool IsActive { get; set; } = true;

    // registrerar när rummet lades till i systemet /saknades DateTime i RoomDto.
    public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
}