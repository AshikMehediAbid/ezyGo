using ezyGo.Payment.Domain.Managers.Interfaces;
using ezyGo.Payment.Domain.Models;
using ezyGo.Payment.Storage.Entities;
using ezyGo.Payment.Storage.Repositories;
using ezyGo.Payment.Storage.Repositories.Interfaces;
using ezyGo.PdfGenerator.Models;
using ezyGo.PdfGenerator.Services;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using System.Text;
using System.Text.Json;

namespace ezyGo.Payment.Domain.Managers;

public class AamarPayService : IPaymentService
{
    private readonly IPaymentRepository _paymentRepo;
    private readonly ISeatService _seatService;
    private readonly ITicketPdfService _ticketPdfService;
    private readonly IEmailService _emailService;
    private readonly IHttpClientFactory _httpClientFactory;
    private readonly ILogger<AamarPayService> _logger;
    private readonly IConfiguration _configuration;

    private readonly string _storeId;
    private readonly string _signatureKey;
    private readonly string _sandboxApiUrl;
    private readonly string _successUrl;
    private readonly string _failUrl;
    private readonly string _cancelUrl;
    public AamarPayService(IPaymentRepository paymentRepo, ISeatService seatService, ITicketPdfService ticketPdf, IEmailService emailService, IHttpClientFactory httpClientFactory, ILogger<AamarPayService> logger, IConfiguration configuration)
    {
        _paymentRepo = paymentRepo;
        _seatService = seatService;
        _ticketPdfService = ticketPdf;
        _emailService = emailService;
        _httpClientFactory = httpClientFactory;
        _logger = logger;
        _configuration = configuration;

        _storeId = _configuration["AamarPay:StoreId"] ?? "aamarpaytest";
        _signatureKey = _configuration["AamarPay:SignatureKey"] ?? "dbb74894e82415a2f7ff0ec3a97e4183";
        _sandboxApiUrl = _configuration["AamarPay:ApiUrl"] ?? "https://sandbox.aamarpay.com/jsonpost.php";
        _successUrl = _configuration["AamarPay:CallbackUrls:Success"] ?? "https://localhost:7195/api/payment/success";
        _failUrl = _configuration["AamarPay:CallbackUrls:Fail"] ?? "https://localhost:7195/api/payment/fail";
        _cancelUrl = _configuration["AamarPay:CallbackUrls:Cancel"] ?? "https://localhost:7195/api/payment/cancel";
        _seatService = seatService;
    }
    public async Task<string> InitiatePaymentAsync(PaymentRequest paymentRequest)
    {
        try
        {
            var paymentData = new AamarPayRequest
            {
                store_id = _storeId,
                tran_id = Guid.NewGuid().ToString(),
                success_url = _successUrl,
                fail_url = _failUrl,
                cancel_url = _cancelUrl,
                amount = paymentRequest.Fare.ToString("F2"),
                currency = "BDT",
                signature_key = _signatureKey,
                desc = "Book Purchase Payment",
                cus_name = paymentRequest.PassengerName ?? "anonymous",
                cus_email = paymentRequest.PassengerEmail ?? "payer@customer.com",
                cus_add1 = "",
                cus_add2 = "Mohakhali DOHS",
                cus_city = "",
                cus_state = "",
                cus_postcode = "",
                cus_country = "Bangladesh",
                cus_phone = paymentRequest.PassengerPhone,
                type = "json"
            };

            var httpClient = _httpClientFactory.CreateClient();
            var jsonContent = JsonSerializer.Serialize(paymentData);
            var content = new StringContent(jsonContent, Encoding.UTF8, "application/json");

            var response = await httpClient.PostAsync(_sandboxApiUrl, content);
            var responseString = await response.Content.ReadAsStringAsync();

            if (!response.IsSuccessStatusCode)
            {
                _logger.LogError("AamarPay API call failed: {StatusCode}", response.StatusCode);
                throw new Exception($"Payment initiation failed: {response.StatusCode}");
            }

            var responseData = JsonSerializer.Deserialize<Dictionary<string, string>>(responseString);

            if (responseData != null && responseData.TryGetValue("payment_url", out var paymentUrl))
            {
                await SavePaymentStatus(paymentRequest, paymentData.tran_id);
                return paymentUrl;
            }
            else if (responseData != null && responseData.TryGetValue("errorMessage", out var errorMessage))
            {
                _logger.LogError("AamarPay error: {ErrorMessage}", errorMessage);
                throw new Exception($"Payment initiation failed: {errorMessage}");
            }
            else
            {
                _logger.LogError("Unexpected response from AamarPay: {Response}", responseString);
                throw new Exception("Payment URL not found in response.");
            }
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error initiating payment");
            throw;
        }
    }

    public async Task<AamarPayValidationResponse> ValidatePaymentAsync(string merTxnId)
    {
        try
        {
            if (string.IsNullOrEmpty(merTxnId))
            {
                throw new ArgumentException("Transaction ID is required");
            }

            string url = $"https://sandbox.aamarpay.com/api/v1/trxcheck/request.php?request_id={merTxnId}&store_id={_storeId}&signature_key={_signatureKey}&type=json";

            var httpClient = _httpClientFactory.CreateClient();
            var response = await httpClient.GetAsync(url);

            if (!response.IsSuccessStatusCode)
            {
                _logger.LogError("Payment validation API call failed: {StatusCode}", response.StatusCode);
                throw new Exception($"Payment validation failed: {response.StatusCode}");
            }

            var json = await response.Content.ReadAsStringAsync();
            var result = JsonSerializer.Deserialize<AamarPayValidationResponse>(json);

            if (result == null)
            {
                throw new Exception("Failed to deserialize validation response");
            }

            return result;
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error validating payment");
            throw;
        }
    }


    private async Task SavePaymentStatus(PaymentRequest paymentRequest, string tran_id)
    {
        var paymentInfo = new PaymentInfo
        {
            TransactionId = tran_id,
            PassengerName = paymentRequest.PassengerName ?? string.Empty,
            PassengerEmail = paymentRequest.PassengerEmail ?? string.Empty,
            PassengerPhone = paymentRequest.PassengerPhone ?? string.Empty,
            BusNumber = paymentRequest.BusNumber ?? string.Empty,
            SeatNumbers = paymentRequest.SeatNumbers ?? string.Empty,
            SeatNames = paymentRequest.SeatNames ?? string.Empty,
            JourneyDate = paymentRequest.JourneyDate,
            From = paymentRequest.From ?? string.Empty,
            To = paymentRequest.To ?? string.Empty,
            Fare = paymentRequest.Fare,
            Status = "Initiated"
        };
        await _paymentRepo.SavePaymentStatus(paymentInfo);
    }

    public async Task UpdatePaymentStatus(string tran_id, string status)
    {
        await _paymentRepo.UpdatePaymentStatus(tran_id, status);
        if (status == "Success")
        {
            // Get all seats from payment info
            var seats = await _paymentRepo.GetSeatsAsync(tran_id);
            await _seatService.ConfirmSeatsAsync(seats);

            // Generate PDF ticket and Send email
            await GenerateTicketAndSendEmail(tran_id, seats);
        }
    }

    private async Task GenerateTicketAndSendEmail(string tran_id, string seats)
    {
        PaymentInfo paymentInfo = await _paymentRepo.GetPaymentInfoByTransactionId(tran_id);
        // Generate pdf
        var ticketModel = new TicketPdfModel
        {
            PassengerName = paymentInfo.PassengerName,
            PassengerEmail = paymentInfo.PassengerEmail,
            PassengerPhone = paymentInfo.PassengerPhone,
            BusNumber = paymentInfo.BusNumber,
            SeatNumbers = paymentInfo.SeatNumbers,
            SeatNames = paymentInfo.SeatNames,
            JourneyDate = paymentInfo.JourneyDate,
            From = paymentInfo.From,
            To = paymentInfo.To,
            Fare = paymentInfo.Fare,
            TicketNo = tran_id
        };
        var pdf = _ticketPdfService.GenerateTicketPdf(ticketModel);

        // Send email with pdf attachment
        await _emailService.SendEmailWithPdf(ticketModel, pdf);
    }

    public async Task<PaymentInfo> GetPaymentInfoByTransactionId(string tran_id)
    {
        var paymentInfo = await _paymentRepo.GetPaymentInfoByTransactionId(tran_id);
        return paymentInfo;
    }
}
