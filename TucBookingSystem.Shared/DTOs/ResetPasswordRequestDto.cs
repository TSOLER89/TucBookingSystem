using System;
using System.Collections.Generic;
using System.Text;

namespace TucBookingSystem.Shared.DTOs
{
    public class ResetPasswordRequestDto
    {
        public string Email { get; set; } = "";
        public string Token { get; set; } = "";
        public string NewPassword { get; set; } = "";
    }
}
