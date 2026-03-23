namespace TucBookingSystem.Api.Services;

public interface IEmailService
{
    Task SendPasswordResetEmailAsync(string toEmail, string resetLink);
    Task SendBookingConfirmationEmailAsync(
        string toEmail,
        string fullName,
        string roomName,
        DateOnly date,
        TimeOnly startTime,
        TimeOnly endTime,
        string purpose);
}
