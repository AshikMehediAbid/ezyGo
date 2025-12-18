using ezyGo.Core.Notification.Email.Models;
using ezyGo.Core.Notification.Email.Services;
using ezyGo.Payment.Domain.Managers.Interfaces;
using ezyGo.PdfGenerator.Models;
using Microsoft.Extensions.Logging;

namespace ezyGo.Payment.Domain.Managers;

public class EmailService : IEmailService
{
    private readonly IEmailSender _emailSender;
    private readonly ILogger<EmailService> _logger;
    public EmailService(IEmailSender emailSender, ILogger<EmailService> logger)
    {
        _emailSender = emailSender;
        _logger = logger;
    }


    public Task SendEmailWithPdf(TicketPdfModel ticketPdfModel, byte[] pdfData)
    {
        try
        {
            var subject = "Your ezyGo Ticket";
            var body = $"Dear {ticketPdfModel.PassengerName},\n\n" +
                       "Thank you for booking with ezyGo. Please find your ticket attached.\n\n" +
                       "Journey Details:\n" +
                       $"From: {ticketPdfModel.From}\n" +
                       $"To: {ticketPdfModel.To}\n" +
                       $"Date: {ticketPdfModel.JourneyDate:dd MMMM yyyy}\n" +
                       $"Bus: {ticketPdfModel.BusNumber}\n" +
                       $"Seat(s): {ticketPdfModel.SeatNames}\n" +
                       $"Fare: ${ticketPdfModel.Fare}\n" +
                       $"Ticket No: {ticketPdfModel.TicketNo}\n\n" +
                       "We wish you a pleasant journey!\n\n" +
                       "Best regards,\n" +
                       "ezyGo Team";
            var attachmentName = $"ezyGo_Ticket_{ticketPdfModel.TicketNo}.pdf";

            var pdfAttachment = new FileAttachment
            {
                FileName = attachmentName,
                ContentType = "application/pdf",
                Content = pdfData
            };

            var emailMessage = new EmailMessage
            {
                To = ticketPdfModel.PassengerEmail,
                Subject = subject,
                Body = body,
                Attachments = pdfAttachment
            };

            return _emailSender.SendEmailAsync(emailMessage);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Failed to send email with PDF ticket to {Email}", ticketPdfModel.PassengerEmail);
            throw;
        }
    }
}
