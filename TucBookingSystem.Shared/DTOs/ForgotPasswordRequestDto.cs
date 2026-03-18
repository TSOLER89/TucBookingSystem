using System.ComponentModel.DataAnnotations;

namespace TucBookingSystem.Shared.DTOs
{
    public class ForgotPasswordRequestDto
    {
        [Required]
        [EmailAddress]
        public string Email { get; set; } = "";
    }
}
