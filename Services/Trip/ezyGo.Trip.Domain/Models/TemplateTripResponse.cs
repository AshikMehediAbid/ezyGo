namespace ezyGo.Trip.Domain.Models;

public class TemplateTripResponse
{
    public int TripTemplateId { get; set; }
    public string Description { get; set; } = string.Empty;

    public int? BusId { get; set; }
    public string BusName { get; set; } = string.Empty;
    public string TotalCapacity { get; set; } = string.Empty;
    public string BusType { get; set; } = string.Empty;
    public int BusCompanyId { get; set; }
    public string CompanyName { get; set; } = string.Empty;

    public int RouteId { get; set; }
    public string StartingPoint { get; set; } = string.Empty;
    public string EndingPoint { get; set; } = string.Empty;

    public int BaseFare { get; set; }

    public TimeOnly DepartureTime { get; set; }
    public TimeOnly ArrivalTime { get; set; }

    public string? Stoppages { get; set; } = String.Empty;
    public List<string> StoppageList { get; set; } = [];

}
