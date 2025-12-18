using ezyGo.Core.Notification.Email.Models;

namespace ezyGo.Core.Notification.Email.Services;

public interface IEmailSender
{
    Task SendEmailAsync(EmailMessage emailMessage );
}
