namespace ezyGo.Admin.Domain.Models;

public class Station
{
    public int Id { get; set; }
    public string StationName { get; set; } = string.Empty;
    public string StationDescription { get; set; } = string.Empty;
    public Geo? Geo { get; set; }
}

public class Geo
{
    public decimal Latitude { get; set; }
    public decimal Longitude { get; set; }
}
