namespace ezyGo.Payment.Domain.Models;

public class PaymentRequest
{
    public string CustomerName { get; set; }
    public string CustomerEmail { get; set; }
    public string CustomerPhone { get; set; }
    public string SelectedSeats { get; set; }
    public int TotalAmount { get; set; }
}
