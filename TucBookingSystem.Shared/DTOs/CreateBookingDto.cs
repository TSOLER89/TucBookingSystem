namespace TucBookingSystem.Shared.DTOs;

public class CreateBookingDto
{
    public int RoomId { get; set; }
    public DateOnly Date { get; set; }
    public TimeOnly StartTime { get; set; }
    public TimeOnly EndTime { get; set; }
    public string Purpose { get; set; } = string.Empty;
}