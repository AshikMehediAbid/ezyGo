using Microsoft.EntityFrameworkCore;

namespace ezyGo.Admin.Storage.Entities;

public class BusStationEntity
{
    public int Id { get; set; }
    public string StationName { get; set; } = string.Empty;
    public string StationDescription { get; set; } = string.Empty;
    public Geo? Geo { get; set; }

    public ICollection<RouteStoppageEntity> RouteStoppages { get; set; } = [];

    public DateTime CreatedAt { get; set; }
    public DateTime UpdatedAt { get; set; }
    public bool IsActive { get; set; } = true;
}
