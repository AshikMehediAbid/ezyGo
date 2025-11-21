namespace ezyGo.Core.ServiceClients.VehicleClient.Models;

public class VehicleSearchRequestClientModel
{
    public VehicleType VehicleType { get; set; }
    public Guid FromStationId { get; set; }
    public Guid ToStationId { get; set; }
    public DateTime JourneyDate { get; set; }
}

public enum VehicleType
{
    Bus = 1,
    Train = 2
}