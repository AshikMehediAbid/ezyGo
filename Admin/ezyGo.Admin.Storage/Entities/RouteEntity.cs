namespace ezyGo.Admin.Storage.Entities;

public class RouteEntity
{
    public int Id { get; set; }
    public BusStationEntity? StartingPoint { get; set; }
    public BusStationEntity? EndingingPoint { get; set; }
}
