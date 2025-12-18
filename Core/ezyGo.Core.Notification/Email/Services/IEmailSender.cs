namespace ezyGo.Core.Notification.Email.Services;

public interface IEmailSender
{
    Task SendEmailAsync(string toEmail, string subject, string body);
}
