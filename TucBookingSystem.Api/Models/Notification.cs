using System.ComponentModel.DataAnnotations;

namespace TucBookingSystem.Api.Models;

public class Notification
{
    public int Id { get; set; }

    [Required]
    public int UserId { get; set; }

    public User? User { get; set; }

    [Required]
    [MaxLength(500)]
    public string Message { get; set; } = string.Empty;

    public bool IsRead { get; set; } = false;

    public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
}
