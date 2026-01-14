using PaymentClient = ezyGo.Core.ServiceClients.PaymentClient;
using ezyGo.PdfGenerator.Services;
using ezyGo.Platform.Domain.Managers;
using ezyGo.QrCodeGenerator.Services;
using Moq;
using ezyGo.PdfGenerator.Models;

namespace ezyGo.Platform.Test.UnitTests;

public class UtilityServiceTests
{
    private readonly Mock<PaymentClient.Clients.IPaymentClient> _paymentClientMock;
    private readonly Mock<ITicketPdfService> _ticketPdfServiceMock;
    private readonly Mock<IQrCodeService> _qrCodeServiceMock;
    private readonly UtilityService _utilityService;


    public UtilityServiceTests()
    {
        _paymentClientMock = new Mock<PaymentClient.Clients.IPaymentClient>();
        _ticketPdfServiceMock = new Mock<ITicketPdfService>();
        _qrCodeServiceMock = new Mock<IQrCodeService>();

        _utilityService = new UtilityService(
            _paymentClientMock.Object,
            _ticketPdfServiceMock.Object,
            _qrCodeServiceMock.Object);
    }


    [Fact]
    public async Task GenerateTicketPdfAsync_WhenTransactionIdNullOrWhiteSpace_ThrowArgumentException()
    {
        // Arrange
        string transactionId = "   ";

        // Act & Assert
        await Assert.ThrowsAsync<ArgumentException>(async () => await _utilityService.GenerateTicketPdfAsync(transactionId));
        await Assert.ThrowsAsync<ArgumentException>(async () => await _utilityService.GenerateTicketPdfAsync(string.Empty));

        // Verify
        _paymentClientMock.Verify(pc => pc.GetPaymentInfoByTransactionIdAsync(It.IsAny<string>()), Times.Never);
    }


    [Fact]
    public async Task GenerateTicketPdfAsync_WhenPaymentInfoNotFound_ThrowKeyNotFoundException()
    {
        // Arrange
        string transactionId = "TX12345";

        _paymentClientMock
            .Setup(pc => pc.GetPaymentInfoByTransactionIdAsync(transactionId))
            .ReturnsAsync((PaymentClient.Models.PaymentInfoClientResponse?)null);

        // Act & Assert
        await Assert.ThrowsAsync<KeyNotFoundException>(async () => await _utilityService.GenerateTicketPdfAsync(transactionId));
    }



    [Fact]
    public async Task GenerateTicketPdfAsync_WhenValidTransactionId_ReturnsPdfBytes()
    {
        // Arrange
        string transactionId = "TX12345";

        var paymentInfo = new PaymentClient.Models.PaymentInfoClientResponse
        {
            TransactionId = transactionId,
            PassengerName = "Test Name",
            PassengerEmail = "test@gamil.com"
        };

        // byte[] expectedPdfBytes = new byte[] { 0x25, 0x50, 0x44, 0x46 }; // Sample PDF bytes
        byte[] expectedPdfBytes = "%PDF"u8.ToArray(); // Use UTF-8 string literal

        _paymentClientMock
            .Setup(pc => pc.GetPaymentInfoByTransactionIdAsync(transactionId))
            .ReturnsAsync(paymentInfo);

        _qrCodeServiceMock
            .Setup(qr => qr.GenerateQrCode(It.IsAny<string>()))
            .Returns([0x01, 0x02, 0x03]); // Sample QR code bytes


        _ticketPdfServiceMock
            .Setup(tp => tp.GenerateTicketPdf(It.IsAny<TicketPdfModel>()))
            .Returns(expectedPdfBytes);

        // Act
        var result = await _utilityService.GenerateTicketPdfAsync(transactionId);

        // Assert
        Assert.Equal(expectedPdfBytes, result);

        // Verify that the mocks were called as expected
        _paymentClientMock.Verify(pc => pc.GetPaymentInfoByTransactionIdAsync(transactionId), Times.Once);
        _qrCodeServiceMock.Verify(qr => qr.GenerateQrCode(It.Is<string>(s => s.Contains(transactionId))), Times.Once);
        _ticketPdfServiceMock.Verify(tp => tp.GenerateTicketPdf(It.IsAny<TicketPdfModel>()), Times.Once);

    }
}