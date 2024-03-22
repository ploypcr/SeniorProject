using Application.Abstractions.Services;
using MailKit.Net.Smtp;
using MailKit.Security;
using Microsoft.Extensions.Configuration;
using MimeKit;

public class EmailService : IEmailService
{
    private readonly IConfiguration _configuration;
    public EmailService(IConfiguration configuration){
        _configuration = configuration;
    }
    public async Task SendEmail(string To, string body)
    {
        var email = new MimeMessage();

        email.From.Add(new MailboxAddress("KU Vet Learning Lab", _configuration.GetSection("EmailConfig:Email").Value));
        email.To.Add(new MailboxAddress(To,To));

        email.Subject = "Testing out email sending";
        email.Body = new TextPart(MimeKit.Text.TextFormat.Plain) { 
            Text = "Hello all the way from the land of C#"
        };
        using (var smtp = new SmtpClient())
        {
            await smtp.ConnectAsync("smtp.gmail.com", 587, SecureSocketOptions.StartTls);
            // Note: only needed if the SMTP server requires authentication
            await smtp.AuthenticateAsync(_configuration.GetSection("EmailConfig:Email").Value, _configuration.GetSection("EmailConfig:Password").Value);
            await smtp.SendAsync(email);
            smtp.Disconnect(true);
        }
    }
}