namespace ezyGo.Admin.Domain.Models;

public class TripTemplateModel
{
    public int Id { get; set; }
    public string Description { get; set; }

    public int? BusId { get; set; }
    public Bus? Bus { get; set; }

    public int? RouteId { get; set; }
    public Route? Route { get; set; }

    public int BaseFare { get; set; }

    public DateTime DepartureTime { get; set; }
    public DateTime ArrivalTime { get; set; }

    public string? Stoppages { get; set; } = String.Empty;

    public DateTime CreatedAt { get; set; }
    public DateTime UpdatedAt { get; set; }
    public bool IsActive { get; set; } = true;

}
