namespace ezyGo.Admin.Storage.Entities;

public class BusEntity
{
    public int Id { get; set; }
    public string BusName { get; set; } = string.Empty;
    public string BusRegNo { get; set; } = string.Empty;
    public string TotalCapacity {  get; set; } = string.Empty;
    public BusType BusType { get; set; }
    public string DriverName { get; set; } = string.Empty;
    public DateTime CreatedAt { get; set; } = DateTime.Now;

    public int? BusCompanyEntityId { get; set; }
    public BusCompanyEntity? Company { get; set; } = null;
    
}
public enum BusType
{
    AC = 1,
    NONAC = 2,
}