namespace ezyGo.Admin.Domain.Models;

public class Vehicle
{
    public int VehicleId { get; set; }
    public string TrainName { get; set; }
    public string TrainDescription { get; set; }
    public int TotalSeat { get; set; }
    public ICollection<VehicleStation> TrainStations { get; set; } = new List<VehicleStation>();

}
