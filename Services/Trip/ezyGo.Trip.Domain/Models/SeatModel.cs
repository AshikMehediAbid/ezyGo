namespace ezyGo.Trip.Domain.Models;

public class SeatModel
{
    public int Id { get; set; }
    public int tripId { get; set; }
    public string SeatNumber { get; set; } = string.Empty;
    public bool IsAvailable { get; set; } = true;
    public int SeatFare { get; set; }
    public int SeatRow { get; set; }
    public int SeatColumn { get; set; }
}
