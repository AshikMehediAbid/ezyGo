using System.ComponentModel.DataAnnotations;

namespace ezyGo.Trip.Storage.Entities;

public class TripDetails
{
    public int Id { get; set; }
    public int TripTemplateId { get; set; }
    public string Description { get; set; } = string.Empty;

    public int? BusId { get; set; }
    public string BusName { get; set; } = string.Empty;
    public string TotalCapacity { get; set; } = string.Empty;
    public string BusType { get; set; } = string.Empty;
    public int CompanyId { get; set; }
    public string CompanyName { get; set; } = string.Empty;

    public int RouteId { get; set; }
    public string StartingPoint { get; set; } = string.Empty;
    public string EndingPoint { get; set; } = string.Empty;

    public int BaseFare { get; set; }

    public TimeOnly DepartureTime { get; set; }
    public TimeOnly ArrivalTime { get; set; }

    // Travel date for the scheduled trip
    [Required]
    public DateOnly TravelDate { get; set; }

    public string? Stoppages { get; set; } = String.Empty;
    public List<string> StoppageList { get; set; } = [];
}
