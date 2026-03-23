using System.ComponentModel.DataAnnotations;

namespace TucBookingSystem.Shared.DTOs;

public class UpdateBookingDto
{
    [Required]
    public DateOnly Date { get; set; }

    [Required]
    public TimeOnly StartTime { get; set; }

    [Required]
    public TimeOnly EndTime { get; set; }

    [MaxLength(200)]
    public string Purpose { get; set; } = string.Empty;
}
