using EMDR42.API.Services.Interfaces;
using EMDR42.Domain.Commons.DTO;
using EMDR42.Domain.Commons.Response;
using EMDR42.Domain.Commons.Singleton;
using MailKit.Security;
using Microsoft.Extensions.Options;
using MimeKit;

namespace EMDR42.API.Services.Implementation;

public class EmailService : IEmailService
{
    private readonly Config _config;
    public EmailService(Config config)
    {
        _config = config;
    }

    public async Task<BaseResponseMessage> SendEmail(SendEmailDto data, CancellationToken ct = default)
    {
        if (String.IsNullOrEmpty(_config.SmtpHost) || String.IsNullOrEmpty(_config.SmtpPort) ||
            String.IsNullOrEmpty(_config.SmtpEmail) || String.IsNullOrEmpty(_config.SmtpPassword))
            return new BaseResponseMessage { StatusCode = 400, Description = "Settings email SmtpClient error!" };
        var emailMessage = new MimeMessage
        {
            Subject = data.Subject,
        };
        emailMessage.From.Add(new MailboxAddress("", _config.SmtpEmail));
        emailMessage.To.Add(new MailboxAddress("", data.Email));


        var builder = new BodyBuilder
        {
            HtmlBody = data.MessageBody
        };

        var a = new List<Stream>();

        emailMessage.Body = builder.ToMessageBody();
        try
        {
            using var smtpClient = new MailKit.Net.Smtp.SmtpClient();
            await smtpClient.ConnectAsync(_config.SmtpHost, Convert.ToInt32(_config.SmtpPort), SecureSocketOptions.Auto, ct);
            await smtpClient.AuthenticateAsync(_config.SmtpEmail, _config.SmtpPassword, ct);
            await smtpClient.SendAsync(emailMessage, ct);
            await smtpClient.DisconnectAsync(true, ct);
            return new BaseResponseMessage { StatusCode = 200, Description = "Success" };
        }
        catch (Exception)
        {
            return new BaseResponseMessage { StatusCode = 400, Description = "Send email error!" };
        }
        finally
        {
            if (a.Any())
            {
                foreach (var stream in a)
                {
                    stream.Close();
                }
            }
        }
    }
}
