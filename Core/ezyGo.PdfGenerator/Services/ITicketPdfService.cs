using ezyGo.PdfGenerator.Models;

namespace ezyGo.PdfGenerator.Services;

public interface ITicketPdfService
{
    byte[] GenerateTicketPdf(TicketPdfModel model);
}
