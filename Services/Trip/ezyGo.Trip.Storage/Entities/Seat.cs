namespace ezyGo.Trip.Storage.Entities;

public class Seat
{
    public int Id { get; set; }
    public int TripId { get; set; }
    public string SeatNumber { get; set; } = string.Empty;
    public bool IsAvailable { get; set; } = true;
    public int SeatFare { get; set; }
    public int SeatRow { get; set; }
    public int SeatColumn { get; set; }
}
