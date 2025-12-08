namespace ezyGo.Admin.Domain.Models;

public class TripTemplateModel
{
    public int Id { get; set; }
    public string Description { get; set; } = string.Empty;

    public int? BusId { get; set; }
    public Bus? Bus { get; set; }

    public int? RouteId { get; set; }
    public Route? Route { get; set; }

    public int BaseFare { get; set; }

    public TimeOnly DepartureTime { get; set; }
    public TimeOnly ArrivalTime { get; set; }

    public bool IsAutoScheduled { get; set; }

    public string? Stoppages { get; set; } = String.Empty;

    public DateTime CreatedAt { get; set; }
    public DateTime UpdatedAt { get; set; }
    public bool IsActive { get; set; } = true;

}
