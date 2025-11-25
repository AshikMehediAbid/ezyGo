using Microsoft.EntityFrameworkCore;

namespace ezyGo.Admin.Storage.Entities;

public class TrainStation
{
    public int TrainStationId { get; set; }
    public string TrainStationName { get; set; }
    public string TrainStationDescription { get; set; }
    public Geo Geo { get; set; }
}

[Owned]
public class Geo
{
    public decimal Latitude { get; set; }
    public decimal Longitude { get; set; }
}
