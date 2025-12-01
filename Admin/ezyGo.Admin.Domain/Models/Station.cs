namespace ezyGo.Admin.Domain.Models;

public class Station
{
    public int Id { get; set; }
    public string StationName { get; set; } = string.Empty;
    public string StationDescription { get; set; } = string.Empty;
    public Geo? Geo { get; set; }

    public DateTime CreatedAt { get; set; }
    public DateTime UpdatedAt { get; set; }
    public bool IsActive { get; set; } = true;
}

public class Geo
{
    public decimal Latitude { get; set; }
    public decimal Longitude { get; set; }
}
