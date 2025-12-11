namespace ezyGo.Core.ServiceClients.TripClient.Models;

public class TripClientRequest
{
    public string FromLocation { get; set; } = string.Empty;
    public string ToLocation { get; set; } = string.Empty;
    public DateOnly TripDate { get; set; }
}
