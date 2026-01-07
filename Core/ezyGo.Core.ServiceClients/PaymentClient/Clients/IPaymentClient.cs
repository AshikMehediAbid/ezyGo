using ezyGo.Core.ServiceClients.PaymentClient.Models;

namespace ezyGo.Core.ServiceClients.PaymentClient.Clients;

public interface IPaymentClient
{
    Task<PaymentInfoClientResponse?> GetPaymentInfoByTransactionIdAsync(string transactionId);
}


