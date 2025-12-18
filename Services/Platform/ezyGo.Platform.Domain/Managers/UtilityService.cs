using ezyGo.Core.ServiceClients.PaymentClient.Clients;
using ezyGo.PdfGenerator.Models;
using ezyGo.PdfGenerator.Services;
using ezyGo.Platform.Domain.Managers.Interfaces;

namespace ezyGo.Platform.Domain.Managers;

public class UtilityService : IUtilityService
{
    private readonly IPaymentClient _paymentClient;
    private readonly ITicketPdfService _ticketPdfService;

    public UtilityService(IPaymentClient paymentClient, ITicketPdfService ticketPdfService)
    {
        _paymentClient = paymentClient;
        _ticketPdfService = ticketPdfService;
    }

    public async Task<byte[]> GenerateTicketPdfAsync(string transactionId)
    {
        if (string.IsNullOrWhiteSpace(transactionId))
            throw new ArgumentException("Transaction ID is required");

        // Get payment info
        var paymentInfo =
            await _paymentClient.GetPaymentInfoByTransactionIdAsync(transactionId);

        if (paymentInfo == null)
            throw new KeyNotFoundException(
                "Payment information not found for the given transaction ID");

        // Build ticket model
        var ticketModel = new TicketPdfModel
        {
            TicketNo = paymentInfo.TransactionId,
            PassengerName = paymentInfo.PassengerName,
            PassengerEmail = paymentInfo.PassengerEmail,
            PassengerPhone = paymentInfo.PassengerPhone,
            BusNumber = paymentInfo.BusNumber,
            SeatNumbers = paymentInfo.SeatNumbers,
            SeatNames = paymentInfo.SeatNames,
            JourneyDate = paymentInfo.JourneyDate,
            From = paymentInfo.From,
            To = paymentInfo.To,
            Fare = paymentInfo.Fare
        };

        // Generate PDF
        return _ticketPdfService.GenerateTicketPdf(ticketModel);
    }
}