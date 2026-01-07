using ezyGo.PdfGenerator.Models;
using QuestPDF.Fluent;
using QuestPDF.Helpers;
using QuestPDF.Infrastructure;

namespace ezyGo.PdfGenerator.Services;

public class TicketPdfService : ITicketPdfService
{
    public byte[] GenerateTicketPdf(TicketPdfModel model)
    {
        return Document.Create(container =>
        {
            container.Page(page =>
            {
                page.Size(PageSizes.A4);
                page.Margin(30);
                page.DefaultTextStyle(x => x.FontSize(12));

                page.Content().Column(col =>
                {
                    /* ================= HEADER ================= */
                    col.Item().Background(Colors.Blue.Darken2).Padding(15).Row(row =>
                    {
                        row.RelativeItem().Text("🚌 ezyGo Bus Ticket")
                            .FontSize(20)
                            .Bold()
                            .FontColor(Colors.White);

                        row.ConstantItem(150).AlignRight().Text($"Ticket No\n{model.TicketNo}")
                            .FontColor(Colors.White)
                            .Bold();
                    });

                    col.Item().PaddingVertical(10);

                    /* ================= PASSENGER INFO ================= */
                    col.Item().Text("Passenger Information")
                        .Bold()
                        .FontSize(14);

                    col.Item().LineHorizontal(1);

                    col.Item().PaddingVertical(5).Table(table =>
                    {
                        table.ColumnsDefinition(columns =>
                        {
                            columns.RelativeColumn();
                            columns.RelativeColumn();
                        });

                        table.Cell().Text($"Name: {model.PassengerName}");
                        table.Cell().Text($"Phone: {model.PassengerPhone}");

                        table.Cell().Text($"Email: {model.PassengerEmail}");
                        table.Cell().Text($"Seat(s): {model.SeatNames}");
                    });

                    col.Item().PaddingVertical(10);

                    /* ================= JOURNEY INFO ================= */
                    col.Item().Text("Journey Details")
                        .Bold()
                        .FontSize(14);

                    col.Item().LineHorizontal(1);

                    col.Item().PaddingVertical(5).Table(table =>
                    {
                        table.ColumnsDefinition(columns =>
                        {
                            columns.RelativeColumn();
                            columns.RelativeColumn();
                        });

                        table.Cell().Text($"From: {model.From}");
                        table.Cell().Text($"To: {model.To}");

                        table.Cell().Text($"Bus: {model.BusNumber}");
                        table.Cell().Text($"Journey Date: {model.JourneyDate:dd MMM yyyy}");
                    });

                    col.Item().PaddingVertical(15);


                    /* ================= QR CODE ================= */
                    if (model.QrCodeImage is { Length: > 0 })
                    {
                        col.Item()
                        .AlignCenter()
                        .Height(200)
                        .Width(200)
                        .Image(model.QrCodeImage, ImageScaling.FitArea);

                        col.Item()
                           .AlignCenter()
                           .Text("Scan this QR code")
                           .Italic()
                           .FontColor(Colors.Grey.Darken1)
                           .FontSize(10);
                    }

                    col.Item().PaddingVertical(30);

                    /* ================= FARE SECTION ================= */
                    col.Item().Background(Colors.Grey.Lighten3).Padding(15).Row(row =>
                    {
                        row.RelativeItem().Text("Total Fare")
                            .Bold()
                            .FontSize(14);

                        row.ConstantItem(150).AlignRight().Text($"{model.Fare} BDT")
                            .Bold()
                            .FontSize(16)
                            .FontColor(Colors.Green.Darken2);
                    });

                    col.Item().PaddingVertical(20);

                    /* ================= FOOTER ================= */
                    col.Item().AlignCenter().Text("Please carry this ticket during your journey")
                        .Italic()
                        .FontColor(Colors.Grey.Darken1);

                    col.Item().AlignCenter().Text("Thank you for choosing ezyGo 🚍")
                        .Bold();
                });
            });
        })
        .GeneratePdf();
    }
}