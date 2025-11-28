namespace ezyGo.Admin.Storage.Entities;

public class RouteEntity
{
    public int Id { get; set; }
    public int? StartingPointId { get; set; }
    public BusStationEntity? StartingPoint { get; set; }
    public int? EndingPointId { get; set; }
    public BusStationEntity? EndingPoint { get; set; }

    public ICollection<RouteStoppageEntity> Stoppages { get; set; } = [];
}
