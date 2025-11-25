namespace ezyGo.Admin.Domain.Models;

public class Station
{
    public int id { get; set; }
    public string StationName { get; set; }
    public string StationDescription { get; set; }
    public Geo Geo { get; set; }
}

public class Geo
{
    public decimal Latitude { get; set; }
    public decimal Longitude { get; set; }
}
