namespace ezyGo.Trip.Domain.Models;

public class UserTripRequest
{
    public string FromLocation { get; set; } = string.Empty;
    public string ToLocation { get; set; } = string.Empty;
    public DateOnly TripDate { get; set; }
}
