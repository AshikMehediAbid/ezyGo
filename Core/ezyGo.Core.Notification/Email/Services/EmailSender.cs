using ezyGo.Core.Notification.Email.Models;
using System.Net;
using System.Net.Mail;

namespace ezyGo.Core.Notification.Email.Services;

public class EmailSender : IEmailSender
{
    public Task SendEmailAsync(EmailMessage emailMessage)
    {
        var smtpClient = new SmtpClient("smtp.gmail.com")
        {
            Port = 587,
            Credentials = new NetworkCredential("mehedi.abid76@gmail.com", "wruy qkvh aslk gkmw"),
            EnableSsl = true
        };

        var mailMessage = new MailMessage
        {
            Subject = emailMessage.Subject,
            Body = emailMessage.Body,
            From = new MailAddress("mehedi.abid76@gmail.com"),
        };
        mailMessage.To.Add(emailMessage.To);

        if (emailMessage.Attachments != null)
        {
            var attachment = new Attachment(
                new MemoryStream(emailMessage.Attachments.Content),
                emailMessage.Attachments.FileName,
                emailMessage.Attachments.ContentType
            );
            mailMessage.Attachments.Add(attachment);
        }

        return smtpClient.SendMailAsync(mailMessage);
    }
}
