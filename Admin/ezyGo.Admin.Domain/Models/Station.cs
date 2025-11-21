namespace ezyGo.Admin.Domain.Models;

public class Station
{
    public string StationName { get; set; }
    public string StationDescription { get; set; }
    public Geo Geo { get; set; }
}

public class Geo
{
    public int Latitude { get; set; }
    public int Longitude { get; set; }
}
