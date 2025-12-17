using ezyGo.Payment.Domain.Models;

namespace ezyGo.Payment.Domain.Managers.Interfaces;

public interface IPaymentService
{
    Task<string> InitiatePaymentAsync(PaymentRequest paymentRequest);
    Task<AamarPayValidationResponse> ValidatePaymentAsync(string merTxnId);
}
