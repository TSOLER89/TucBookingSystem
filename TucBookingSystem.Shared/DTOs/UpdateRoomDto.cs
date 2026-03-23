using System.ComponentModel.DataAnnotations;

namespace TucBookingSystem.Shared.DTOs;

public class UpdateRoomDto
{
    [Required]
    [MaxLength(100)]
    public string Name { get; set; } = string.Empty;

    [Required]
    [MaxLength(100)]
    public string Location { get; set; } = string.Empty;

    [Range(1, 1000, ErrorMessage = "Kapacitet måste vara mellan 1 och 1000.")]
    public int Capacity { get; set; }

    [MaxLength(300)]
    public string Description { get; set; } = string.Empty;

    public bool IsActive { get; set; }
}
