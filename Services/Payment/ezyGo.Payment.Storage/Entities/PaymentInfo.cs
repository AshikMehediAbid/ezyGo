namespace ezyGo.Payment.Storage.Entities;

public class PaymentInfo
{
    public int Id { get; set; }
    public string TransactionId { get; set; }
    public int Amount { get; set; }
    public string Seats { get; set; }
    public string Status { get; set; }
}
