using MailKit.Net.Smtp;
using MimeKit;
using System.Globalization;

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

    private string GetRequiredEmailSetting(string key)
    {
        return _configuration[key]
            ?? throw new InvalidOperationException($"Missing email configuration value: {key}");
    }

    public async Task SendPasswordResetEmailAsync(string toEmail, string resetLink)
    {
        try
        {
            var message = new MimeMessage();
            message.From.Add(new MailboxAddress(
                GetRequiredEmailSetting("Email:FromName"),
                GetRequiredEmailSetting("Email:FromAddress")
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
                GetRequiredEmailSetting("Email:SmtpServer"),
                int.Parse(GetRequiredEmailSetting("Email:SmtpPort")),
                MailKit.Security.SecureSocketOptions.StartTls
            );

            // Autentisera
            await client.AuthenticateAsync(
                GetRequiredEmailSetting("Email:Username"),
                GetRequiredEmailSetting("Email:Password")
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

    public async Task SendBookingConfirmationEmailAsync(
        string toEmail,
        string fullName,
        string roomName,
        DateOnly date,
        TimeOnly startTime,
        TimeOnly endTime,
        string purpose)
    {
        try
        {
            var swedishCulture = CultureInfo.GetCultureInfo("sv-SE");
            var formattedDate = date.ToString("dddd d MMMM yyyy", swedishCulture);
            var formattedTime = $"{startTime:HH\\:mm} - {endTime:HH\\:mm}";
            var safePurpose = string.IsNullOrWhiteSpace(purpose) ? "Ingen beskrivning angiven." : purpose;

            var message = new MimeMessage();
            message.From.Add(new MailboxAddress(
                GetRequiredEmailSetting("Email:FromName"),
                GetRequiredEmailSetting("Email:FromAddress")
            ));
            message.To.Add(new MailboxAddress(fullName, toEmail));
            message.Subject = "Bokningsbekräftelse - TUC Booking System";

            var bodyBuilder = new BodyBuilder
            {
                HtmlBody = $@"
                    <html>
                    <body>
                        <h2>Din bokning är bekräftad</h2>
                        <p>Hej {fullName},</p>
                        <p>Din bokning har registrerats i TUC Booking System.</p>
                        <p><strong>Rum:</strong> {roomName}</p>
                        <p><strong>Datum:</strong> {formattedDate}</p>
                        <p><strong>Tid:</strong> {formattedTime}</p>
                        <p><strong>Syfte:</strong> {safePurpose}</p>
                        <br/>
                        <p>Med vänliga hälsningar,<br/>TUC Booking System</p>
                    </body>
                    </html>
                ",
                TextBody = $@"
                    Din bokning är bekräftad

                    Hej {fullName},

                    Din bokning har registrerats i TUC Booking System.

                    Rum: {roomName}
                    Datum: {formattedDate}
                    Tid: {formattedTime}
                    Syfte: {safePurpose}

                    Med vänliga hälsningar,
                    TUC Booking System
                "
            };

            message.Body = bodyBuilder.ToMessageBody();

            using var client = new SmtpClient();

            await client.ConnectAsync(
                GetRequiredEmailSetting("Email:SmtpServer"),
                int.Parse(GetRequiredEmailSetting("Email:SmtpPort")),
                MailKit.Security.SecureSocketOptions.StartTls
            );

            await client.AuthenticateAsync(
                GetRequiredEmailSetting("Email:Username"),
                GetRequiredEmailSetting("Email:Password")
            );

            await client.SendAsync(message);
            await client.DisconnectAsync(true);

            _logger.LogInformation("Booking confirmation email sent successfully to {Email}", toEmail);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Failed to send booking confirmation email to {Email}", toEmail);
            throw;
        }
    }
}
