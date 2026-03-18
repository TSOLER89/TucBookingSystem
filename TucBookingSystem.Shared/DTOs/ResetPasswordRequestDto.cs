using System.ComponentModel.DataAnnotations;

namespace TucBookingSystem.Shared.DTOs
{
    public class ResetPasswordRequestDto
    {
        [Required]
        [EmailAddress]
        public string Email { get; set; } = "";

        [Required]
        public string Token { get; set; } = "";

        [Required]
        [MinLength(6, ErrorMessage = "Lösenordet måste vara minst 6 tecken.")]
        public string NewPassword { get; set; } = "";
    }
}
