using System.Text.Json.Serialization;

namespace ezyGo.Payment.Domain.Models;

public class AamarPayValidationResponse
{
    [JsonPropertyName("result")]
    public bool Result { get; set; }

    [JsonPropertyName("pay_status")]
    public string PayStatus { get; set; }

    [JsonPropertyName("mer_txnid")]
    public string MerTxnId { get; set; }

    [JsonPropertyName("pg_txnid")]
    public string PgTxnId { get; set; }

    [JsonPropertyName("amount")]
    public string Amount { get; set; }

    [JsonPropertyName("bank_txnid")]
    public string BankTxnId { get; set; }

    [JsonPropertyName("card_type")]
    public string CardType { get; set; }

    [JsonPropertyName("card_number")]
    public string CardNumber { get; set; }

    [JsonPropertyName("pay_time")]
    public string PayTime { get; set; }

    [JsonPropertyName("currency")]
    public string Currency { get; set; }

    [JsonPropertyName("card_holder")]
    public string CardHolder { get; set; }

    [JsonPropertyName("store_amount")]
    public string StoreAmount { get; set; }
}
