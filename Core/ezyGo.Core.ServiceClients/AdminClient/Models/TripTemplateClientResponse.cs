using System.Text.Json.Serialization;

namespace ezyGo.Core.ServiceClients.AdminClient.Models;

public class TripTemplateClientResponse
{
    [JsonPropertyName("id")]
    public int TemplateId { get; set; }

    [JsonPropertyName("description")]
    public string TripDescription { get; set; } = string.Empty;

    [JsonPropertyName("busId")]
    public int? BusId { get; set; }

    // Nested: bus.busName → BusName
    [JsonPropertyName("bus")]
    public BusInfo? Bus { get; set; }

    public string BusName => Bus?.BusName ?? string.Empty;
    public string TotalCapacity => Bus?.TotalCapacity ?? string.Empty;
    public int CompanyId => Bus?.BusCompanyId ?? 0;
    public string CompanyName => Bus?.CompanyName ?? string.Empty;

    [JsonPropertyName("routeId")]
    public int? RouteId { get; set; }

    [JsonPropertyName("route")]
    public RouteInfo? Route { get; set; }

    public string StartingPoint => Route?.StartingPoint?.StationName ?? string.Empty;
    public string EndingPoint => Route?.EndingPoint?.StationName ?? string.Empty;

    [JsonPropertyName("baseFare")]
    public int BaseFare { get; set; }

    [JsonPropertyName("departureTime")]
    public TimeOnly DepartureTime { get; set; }

    [JsonPropertyName("arrivalTime")]
    public TimeOnly ArrivalTime { get; set; }

    [JsonPropertyName("stoppages")]
    public string? Stoppages { get; set; } = string.Empty;

    public List<string> StoppagesList =>
        Stoppages?.Split('-', StringSplitOptions.TrimEntries).ToList() ?? new();
}

public class BusInfo
{
    [JsonPropertyName("busName")]
    public string BusName { get; set; } = "";

    [JsonPropertyName("totalCapacity")]
    public string TotalCapacity { get; set; } = "";

    [JsonPropertyName("busCompanyId")]
    public int BusCompanyId { get; set; }

    [JsonPropertyName("companyName")]
    public string CompanyName { get; set; } = "";
}

public class RouteInfo
{
    [JsonPropertyName("startingPoint")]
    public StationInfo? StartingPoint { get; set; }

    [JsonPropertyName("endingPoint")]
    public StationInfo? EndingPoint { get; set; }
}

public class StationInfo
{
    [JsonPropertyName("stationName")]
    public string StationName { get; set; } = "";
}
