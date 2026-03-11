using System.ComponentModel.DataAnnotations;

namespace TucBookingSystem.Api.Models
{
    public class Booking
    {
        public int Id { get; set; }

        [Required]
        public int RoomId { get; set; }

        [Required]
        public int UserId { get; set; }

        [Required]
        public DateTime StartTime { get; set; }

        [Required]
        public DateTime EndTime { get; set; }

        public Room? Room { get; set; }
        public User? User { get; set; }
    }
}
