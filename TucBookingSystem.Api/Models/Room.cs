using System.ComponentModel.DataAnnotations;

namespace TucBookingSystem.Api.Models
{
    public class Room
    {
        public int Id { get; set; }

        [Required]
        [MaxLength(100)]
        public string Name { get; set; } = string.Empty;

        public int Capacity { get; set; }

        [MaxLength(200)]
        public string Location { get; set; } = string.Empty;
    }
}
