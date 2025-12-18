using ezyGo.Core.ServiceClients.PaymentClient.Clients;
using ezyGo.PdfGenerator.Models;
using ezyGo.PdfGenerator.Services;
using ezyGo.Platform.Domain.Managers.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace ezyGo.Platform.Service.Controllers;

[Route("api/utility")]
[ApiController]
public class UtilityController : ControllerBase
{
    private readonly IUtilityService _utilityService;
    private readonly ILogger<UtilityController> _logger;

    public UtilityController(
        IUtilityService utilityService,
        IPaymentClient paymentClient,
        ITicketPdfService ticketPdfService,
        ILogger<UtilityController> logger)
    {
        _utilityService = utilityService;
        _logger = logger;
    }

    [HttpGet("download-ticket/{transactionId}")]
    public async Task<IActionResult> DownloadTicket(string transactionId)
    {
        try
        {
            var pdfBytes = await _utilityService.GenerateTicketPdfAsync(transactionId);

            var fileName = $"ezyGo_Ticket_{transactionId}.pdf";
            return File(pdfBytes, "application/pdf", fileName);
        }
        catch (ArgumentException ex)
        {
            return BadRequest(new { Message = ex.Message });
        }
        catch (KeyNotFoundException ex)
        {
            return NotFound(new { Message = ex.Message });
        }
        catch (Exception ex)
        {
            _logger.LogError(ex,
                "Error generating ticket PDF for transaction {TransactionId}",
                transactionId);

            return StatusCode(500, new
            {
                Success = false,
                Message = "An error occurred while generating the ticket PDF"
            });
        }
    }
}
