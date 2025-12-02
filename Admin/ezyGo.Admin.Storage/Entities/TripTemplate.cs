using System.ComponentModel.DataAnnotations;

namespace ezyGo.Admin.Storage.Entities;

public class TripTemplate
{
    public int Id { get; set; }
    public string Description { get; set; } = string.Empty;

    public int? BusId { get; set; }
    public BusEntity? BusEntity { get; set; }

    public int? RouteId { get; set; }
    public RouteEntity? RouteEntity { get; set; }

    public int BaseFare { get; set; }

    [Required]
    [DataType(DataType.DateTime)]
    public TimeOnly DepartureTime { get; set; }

    [Required]
    [DataType(DataType.DateTime)]
    public TimeOnly ArrivalTime { get; set; }

    public string Stoppages { get; set; } = String.Empty;


    public DateTime CreatedAt { get; set; } 
    public DateTime UpdatedAt { get; set; }
    public bool IsActive { get; set; } = true;
}
