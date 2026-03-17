using MailKit.Net.Smtp;
using MimeKit;

namespace TucBookingSystem.Api.Services;

public class EmailService : IEmailService
{
    private readonly IConfiguration _configuration;
    private readonly ILogger<EmailService> _logger;

    public EmailService(IConfiguration configuration, ILogger<EmailService> logger)
    {
        _configuration = configuration;
        _logger = logger;
    }

    public async Task SendPasswordResetEmailAsync(string toEmail, string resetLink)
    {
        try
        {
            var message = new MimeMessage();
            message.From.Add(new MailboxAddress(
                _configuration["Email:FromName"],
                _configuration["Email:FromAddress"]
            ));
            message.To.Add(new MailboxAddress("", toEmail));
            message.Subject = "Återställ ditt lösenord - TUC Booking System";

            var bodyBuilder = new BodyBuilder
            {
                HtmlBody = $@"
                    <html>
                    <body>
                        <h2>Återställ ditt lösenord</h2>
                        <p>Du har begärt att återställa ditt lösenord.</p>
                        <p>Klicka på länken nedan för att återställa ditt lösenord:</p>
                        <p><a href='{resetLink}'>Återställ lösenord</a></p>
                        <p>Länken är giltig i 30 minuter.</p>
                        <p>Om du inte begärde detta kan du ignorera detta meddelande.</p>
                        <br/>
                        <p>Med vänliga hälsningar,<br/>TUC Booking System</p>
                    </body>
                    </html>
                ",
                TextBody = $@"
                    Återställ ditt lösenord

                    Du har begärt att återställa ditt lösenord.

                    Klicka på länken nedan för att återställa ditt lösenord:
                    {resetLink}

                    Länken är giltig i 30 minuter.

                    Om du inte begärde detta kan du ignorera detta meddelande.

                    Med vänliga hälsningar,
                    TUC Booking System
                "
            };

            message.Body = bodyBuilder.ToMessageBody();

            using var client = new SmtpClient();

            // Anslut till SMTP-server
            await client.ConnectAsync(
                _configuration["Email:SmtpServer"],
                int.Parse(_configuration["Email:SmtpPort"] ?? "587"),
                MailKit.Security.SecureSocketOptions.StartTls
            );

            // Autentisera
            await client.AuthenticateAsync(
                _configuration["Email:Username"],
                _configuration["Email:Password"]
            );

            // Skicka e-post
            await client.SendAsync(message);
            await client.DisconnectAsync(true);

            _logger.LogInformation("Password reset email sent successfully to {Email}", toEmail);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Failed to send password reset email to {Email}", toEmail);
            throw;
        }
    }
}
