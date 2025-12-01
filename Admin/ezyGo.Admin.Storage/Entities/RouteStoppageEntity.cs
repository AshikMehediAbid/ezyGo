namespace ezyGo.Admin.Storage.Entities;

public class RouteStoppageEntity
{
    public int Id { get; set; }
    public int RouteEntityId { get; set; }
    public RouteEntity? RouteEntity { get; set; }

    public int BusStationEntityId { get; set; }
    public BusStationEntity? BusStationEntity { get; set; }

    public int Order { get; set; }

    public DateTime CreatedAt { get; set; }
    public DateTime UpdatedAt { get; set; }
    public bool IsActive { get; set; } = true;

}
