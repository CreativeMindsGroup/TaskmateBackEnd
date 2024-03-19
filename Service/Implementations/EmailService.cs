using MailKit.Security;
using Microsoft.Extensions.Options;
using MimeKit.Text;
using MimeKit;
using MailKit.Net.Smtp;
using TaskMate.Helper.Email;
using TaskMate.Service.Abstraction;

namespace TaskMate.Service.Implementations;

public class EmailService : IEmailService
{
    private readonly EmailSetting _emailSetting;
    private readonly IConfiguration _configuration;

    public EmailService(IOptions<EmailSetting> emailSetting, IConfiguration configuration)
    {
        _emailSetting = emailSetting.Value;
        _configuration = configuration;
    }

    public void Send(string to, string subject, string html, string form = null)
    {
        var email = new MimeMessage();
        email.From.Add(MailboxAddress.Parse(form ?? _emailSetting.From));
        email.To.Add(MailboxAddress.Parse(to));
        email.Subject = subject;
        email.Body = new TextPart(TextFormat.Html) { Text = html };

        using var smtp = new SmtpClient();
        smtp.Connect(_emailSetting.SmtpServer, _emailSetting.Port, SecureSocketOptions.StartTls);
        smtp.Authenticate(_emailSetting.UserName, _emailSetting.Password);
        smtp.Send(email);
        smtp.Disconnect(true);
    }

    public Task SendPasswordResetMailAsync(string to, string userId, string resetToken)
    {
        //StringBuilder mail = new();
        //mail.AppendLine("Merhaba<br>Eğer yeni şifre talebinde bulunduysanız aşağıdaki linkten şifrenizi yenileyebilirsiniz.<br><strong><a target=\"_blank\" href=\"http://localhost:3000/ResertPassword/");
        //mail.AppendLine(userId);
        //mail.AppendLine("/");
        //mail.AppendLine(resetToken);
        //mail.AppendLine("\">Yeni şifre talebi için tıklayınız...</a></strong><br><br><span style=\"font-size:12px;\">NOT : Eğer ki bu talep tarafınızca gerçekleştirilmemişse lütfen bu maili ciddiye almayınız.</span><br>Saygılarımızla...<br><br><br>LD - LuxeDrive");

        //Send(to, "Şifre Yenileme Talebi", mail.ToString());
        throw new NotImplementedException();
    }
}