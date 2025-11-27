using ezyGo.Admin.Storage.Entities;

namespace ezyGo.Admin.Domain.Models;

public class Bus
{
    public int Id { get; set; }
    public string BusName { get; set; } = string.Empty;
    public string BusRegNo { get; set; } = string.Empty;
    public string DriverName { get; set; } = string.Empty;
    public string TotalCapacity { get; set; } = string.Empty;
    public BusType BusType { get; set; }
    public int BusCompanyId { get; set; }
    public string CompanyName { get; set; } = string.Empty;
    public DateTime CreatedAt { get; set; }
}
