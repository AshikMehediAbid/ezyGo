using ezyGo.PdfGenerator.Models;
using QuestPDF.Fluent;

namespace ezyGo.PdfGenerator.Services;

public class TicketPdfService : ITicketPdfService
{
    public byte[] GenerateTicketPdf(TicketPdfModel model)
    {
        return Document.Create(container =>
        {
            container.Page(page =>
            {
                page.Margin(30);

                page.Content().Column(col =>
                {
                    col.Item().Text("Bus Ticket")
                        .FontSize(22).Bold();

                    col.Item().Text($"Ticket No: {model.TicketNo}");
                    col.Item().Text($"Passenger: {model.PassengerName}");
                    col.Item().Text($"Passenger: {model.PassengerEmail}");
                    col.Item().Text($"Passenger: {model.PassengerPhone}");
                    col.Item().Text($"Bus: {model.BusNumber}");
                    col.Item().Text($"Seat: {model.SeatNumbers}");
                    col.Item().Text($"Route: {model.From} → {model.To}");
                    col.Item().Text($"Journey Date: {model.JourneyDate:dd MMM yyyy}");
                    col.Item().Text($"Fare: {model.Fare} BDT");
                });
            });
        }).GeneratePdf();
    }
}
