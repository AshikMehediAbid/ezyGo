using ezyGo.Payment.Domain.Managers;
using ezyGo.Payment.Domain.Managers.Interfaces;
using ezyGo.Payment.Domain.Models;
using Microsoft.AspNetCore.Mvc;

namespace ezyGo.Payment.Service.Controllers;

[Route("api/payment")]
[ApiController]
public class PaymentController : ControllerBase
{
    private readonly IPaymentService _paymentService;
    private readonly ILogger<PaymentController> _logger;

    public PaymentController(IPaymentService paymentService, ILogger<PaymentController> logger)
    {
        _paymentService = paymentService;
        _logger = logger;
    }

    [HttpPost("initiate")]
    public async Task<IActionResult> InitiatePayment([FromBody] PaymentRequest paymentRequest)
    {
        try
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var paymentUrl = await _paymentService.InitiatePaymentAsync(paymentRequest);

            return Ok(new
            {
                Success = true,
                PaymentUrl = paymentUrl,
                Message = "Payment initiated successfully"
            });
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error initiating payment");
            return StatusCode(500, new
            {
                Success = false,
                Message = "An error occurred while initiating payment",
                Error = ex.Message
            });
        }
    }

    [HttpPost("success")]
    public async Task<IActionResult> Success([FromForm] string mer_txnid)
    {
        try
        {
            _logger.LogInformation("✅ Payment Success callback received for Transaction: {TransactionId}", mer_txnid);

            if (string.IsNullOrEmpty(mer_txnid))
            {
                return BadRequest(new
                {
                    Success = false,
                    Message = "Invalid transaction ID"
                });
            }

            var validationResult = await _paymentService.ValidatePaymentAsync(mer_txnid);

            // Update order/payment status in your database here
            // await _orderService.UpdatePaymentStatusAsync(mer_txnid, validationResult);
            // Redirect to frontend with success
            return Redirect($"http://localhost:8080/payment/result?status=success&mer_txnid={mer_txnid}");
            return Ok(new
            {
                Success = true,
                Message = "Payment successful",
                Data = validationResult,
                TransactionId = mer_txnid
            });
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error processing success callback");
            return StatusCode(500, new
            {
                Success = false,
                Message = "An error occurred while processing payment",
                Error = ex.Message
            });
        }
    }

    [HttpPost("fail")]
    public IActionResult Fail([FromForm] string mer_txnid)
    {
        _logger.LogWarning("❌ Payment Failed for Transaction: {TransactionId}", mer_txnid);

        // Update order/payment status to failed in your database
        // await _orderService.UpdatePaymentStatusAsync(mer_txnid, "Failed");

        return Ok(new
        {
            Success = false,
            Message = "Payment failed",
            TransactionId = mer_txnid
        });
    }

    [HttpPost("cancel")]
    public IActionResult Cancel([FromForm] string mer_txnid)
    {
        _logger.LogInformation("⚠️ Payment Cancelled for Transaction: {TransactionId}", mer_txnid);

        // Update order/payment status to cancelled in your database
        // await _orderService.UpdatePaymentStatusAsync(mer_txnid, "Cancelled");

        return Ok(new
        {
            Success = false,
            Message = "Payment cancelled",
            TransactionId = mer_txnid
        });
    }

    [HttpGet("validate/{transactionId}")]
    public async Task<IActionResult> ValidatePayment(string transactionId)
    {
        try
        {
            if (string.IsNullOrEmpty(transactionId))
            {
                return BadRequest(new { Message = "Transaction ID is required" });
            }

            var validationResult = await _paymentService.ValidatePaymentAsync(transactionId);

            return Ok(new
            {
                Success = true,
                Data = validationResult
            });
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error validating payment");
            return StatusCode(500, new
            {
                Success = false,
                Message = "An error occurred while validating payment",
                Error = ex.Message
            });
        }
    }

}
