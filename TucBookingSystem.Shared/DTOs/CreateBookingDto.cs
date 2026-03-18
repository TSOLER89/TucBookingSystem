using System.ComponentModel.DataAnnotations;

namespace TucBookingSystem.Shared.DTOs;

public class CreateBookingDto
{
    [Required]
    [Range(1, int.MaxValue, ErrorMessage = "Du måste välja ett rum.")]
    public int RoomId { get; set; }

    [Required]
    public DateOnly Date { get; set; }

    [Required]
    public TimeOnly StartTime { get; set; }

    [Required]
    public TimeOnly EndTime { get; set; }

    [MaxLength(200)]
    public string Purpose { get; set; } = string.Empty;
}