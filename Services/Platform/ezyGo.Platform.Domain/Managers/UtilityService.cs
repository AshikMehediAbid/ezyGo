using ezyGo.Core.ServiceClients.PaymentClient.Clients;
using ezyGo.PdfGenerator.Models;
using ezyGo.PdfGenerator.Services;
using ezyGo.Platform.Domain.Managers.Interfaces;
using ezyGo.QrCodeGenerator.Services;

namespace ezyGo.Platform.Domain.Managers;

public class UtilityService : IUtilityService
{
    private readonly IPaymentClient _paymentClient;
    private readonly ITicketPdfService _ticketPdfService;
    private readonly IQrCodeService _qrCodeService;

    public UtilityService(IPaymentClient paymentClient, ITicketPdfService ticketPdfService, IQrCodeService qrCodeService)
    {
        _paymentClient = paymentClient;
        _ticketPdfService = ticketPdfService;
        _qrCodeService = qrCodeService;
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

        var qrCodeData = $"Ticket No: {ticketModel.TicketNo} | " +
                         $"Name: {ticketModel.PassengerName} | " +
                         $"Journey Date: {ticketModel.JourneyDate:yyyy-MM-dd} | " +
                         $"Bus: {ticketModel.BusNumber} | " +
                         $"{ticketModel.From} - {ticketModel.To} | " +
                         $"Seat/s: {ticketModel.SeatNames} | "+
                         $"Fare: {ticketModel.Fare} ";

        // Generate QR code
        ticketModel.QrCodeImage = _qrCodeService.GenerateQrCode(qrCodeData);

        // Generate PDF
        return _ticketPdfService.GenerateTicketPdf(ticketModel);
    }
}