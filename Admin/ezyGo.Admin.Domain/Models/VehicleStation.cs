namespace ezyGo.Admin.Domain.Models;

public class VehicleStation
{
    public int Id { get; set; }

    public int VehicleId { get; set; }
    public Vehicle Vehicle { get; set; }

    public int StationId { get; set; }
    public Station Station { get; set; }

    public DateTime CreatedAt { get; set; }
    public DateTime UpdatedAt { get; set; }
    public bool IsActive { get; set; } = true;


}
