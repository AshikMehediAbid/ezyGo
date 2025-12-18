using ezyGo.PdfGenerator.Models;

namespace ezyGo.Payment.Domain.Managers.Interfaces;

public interface IEmailService
{
    Task SendEmailWithPdf(TicketPdfModel ticketPdfModel, byte[] pdfData);
}
