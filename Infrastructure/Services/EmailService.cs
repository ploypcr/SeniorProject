using Application.Abstractions.Services;
using MailKit;
using MailKit.Net.Proxy;
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
    public async Task SendEmail(string to, string id, string token)
    {
        var body = "Please confirm your email address <a href=#URL#>Click here</a>";
        var url =  _configuration.GetSection("EmailConfig:BASE_URL").Value+$"/#/confirmEmail?id={id}&code={token}";
        body = body.Replace("#URL#", url);

        var email = new MimeMessage();
        email.From.Add(new MailboxAddress("KU Vet Learning Lab", _configuration.GetSection("EmailConfig:Email").Value));
        email.To.Add(new MailboxAddress(to,to));

        email.Subject = "KU Vet Learning Lab Email Verification";
        email.Body = new TextPart(MimeKit.Text.TextFormat.Html) { 
            Text = body
        };
        using (var smtp = new SmtpClient(new ProtocolLogger("smtp.log")))
        {
            //smtp.ProxyClient = new Socks5Client("10.3.133.119",5000);
            await smtp.ConnectAsync("smtp.gmail.com", 587, SecureSocketOptions.Auto);
            // Note: only needed if the SMTP server requires authentication
            await smtp.AuthenticateAsync(_configuration.GetSection("EmailConfig:Email").Value, _configuration.GetSection("EmailConfig:Password").Value);
            await smtp.SendAsync(email);

            await smtp.DisconnectAsync(true);
        }
    }
}