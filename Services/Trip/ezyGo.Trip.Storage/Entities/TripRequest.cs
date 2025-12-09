namespace ezyGo.Trip.Storage.Entities;

public class TripRequest
{
    public string FromLocation { get; set; } = string.Empty;
    public string ToLocation { get; set; } = string.Empty;
    public DateOnly TripDate { get; set; }
}
