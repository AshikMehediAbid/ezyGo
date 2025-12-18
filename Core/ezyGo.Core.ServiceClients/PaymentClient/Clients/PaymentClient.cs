using ezyGo.Core.ServiceClients.PaymentClient.Models;
using System.Net.Http.Json;

namespace ezyGo.Core.ServiceClients.PaymentClient.Clients;

public class PaymentClient : IPaymentClient
{
    private readonly HttpClient _httpClient;

    public PaymentClient(HttpClient httpClient)
    {
        _httpClient = httpClient;
    }

    public async Task<PaymentInfoClientResponse?> GetPaymentInfoByTransactionIdAsync(string transactionId)
    {
        var response = await _httpClient.GetAsync($"api/payment/info/{transactionId}");

        if (!response.IsSuccessStatusCode)
        {
            if (response.StatusCode == System.Net.HttpStatusCode.NotFound)
            {
                return null;
            }
            throw new HttpRequestException($"Request failed with status code {response.StatusCode}");
        }

        var result = await response.Content.ReadFromJsonAsync<PaymentClientResponseWrapper>();

        return result?.Data;
    }

    private class PaymentClientResponseWrapper
    {
        public bool Success { get; set; }
        public PaymentInfoClientResponse? Data { get; set; }
    }
}

