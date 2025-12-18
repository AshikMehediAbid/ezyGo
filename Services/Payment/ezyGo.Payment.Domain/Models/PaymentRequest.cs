namespace ezyGo.Payment.Domain.Models;

public class PaymentRequest
{
    public string PassengerName { get; set; }
    public string PassengerEmail { get; set; }
    public string PassengerPhone { get; set; }
    public string BusNumber { get; set; }
    public string SeatNumbers { get; set; }
    public string SeatNames { get; set; }
    public DateTime JourneyDate { get; set; }
    public string From { get; set; }
    public string To { get; set; }
    public int Fare { get; set; }
    public string TicketNo { get; set; }
}
