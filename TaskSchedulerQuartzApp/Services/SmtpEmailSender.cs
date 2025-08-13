using MailKit.Security;
using Microsoft.Extensions.Options;
using MimeKit;
using TaskSchedulerQuartzApp.Models;
using MailKit.Net.Smtp;



namespace TaskSchedulerQuartzApp.Services
{
    
    public class SmtpEmailSender(IOptions<EmailSettings> options) : IEmailSender
    {
        private readonly EmailSettings _settings = options.Value;

        public async Task SendAsync(string subject, string htmlBody, CancellationToken ct = default)
        {
            var message = new MimeMessage();
            message.From.Add(new MailboxAddress(_settings.FromName, _settings.FromEmail));

            var toList = (_settings.ToEmails ?? "")
                .Split(new[] { ';', ',' }, System.StringSplitOptions.RemoveEmptyEntries)
                .Select(x => x.Trim());

            foreach (var to in toList)
                message.To.Add(MailboxAddress.Parse(to));

            message.Subject = subject;
            var builder = new BodyBuilder { HtmlBody = htmlBody };
            message.Body = builder.ToMessageBody();

            using var client = new SmtpClient();
            var secure = _settings.UseSsl ? SecureSocketOptions.SslOnConnect : SecureSocketOptions.StartTlsWhenAvailable;

            await client.ConnectAsync(_settings.SmtpHost, _settings.SmtpPort, secure, ct);
            if (!string.IsNullOrWhiteSpace(_settings.UserName))
                await client.AuthenticateAsync(_settings.UserName, _settings.Password, ct);

            await client.SendAsync(message, ct);
            await client.DisconnectAsync(true, ct);
        }
    }
}
