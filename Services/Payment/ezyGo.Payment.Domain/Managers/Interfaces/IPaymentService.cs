using ezyGo.Payment.Domain.Models;
using ezyGo.Payment.Storage.Entities;

namespace ezyGo.Payment.Domain.Managers.Interfaces;

public interface IPaymentService
{
    Task<string> InitiatePaymentAsync(PaymentRequest paymentRequest);
    Task<AamarPayValidationResponse> ValidatePaymentAsync(string merTxnId);
    Task<PaymentInfo> GetPaymentInfoByTransactionId(string tran_id);
    Task UpdatePaymentStatus(string tran_id, string status);
}
