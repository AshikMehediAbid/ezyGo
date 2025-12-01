namespace ezyGo.Admin.Domain.Models;

public class Vehicle
{
    public int VehicleId { get; set; }
    public string TrainName { get; set; }
    public string TrainDescription { get; set; }
    public int TotalSeat { get; set; }
    public ICollection<VehicleStation> TrainStations { get; set; } = new List<VehicleStation>();

    public DateTime CreatedAt { get; set; }
    public DateTime UpdatedAt { get; set; }
    public bool IsActive { get; set; } = true;

}
