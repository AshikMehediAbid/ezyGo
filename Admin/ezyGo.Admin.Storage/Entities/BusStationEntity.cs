using Microsoft.EntityFrameworkCore;

namespace ezyGo.Admin.Storage.Entities;

public class BusStationEntity
{
    public int Id { get; set; }
    public string StationName { get; set; }
    public string StationDescription { get; set; }
    public Geo Geo { get; set; }
}
