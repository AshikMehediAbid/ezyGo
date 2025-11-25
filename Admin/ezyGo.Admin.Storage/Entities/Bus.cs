namespace ezyGo.Admin.Storage.Entities;

public class Bus
{
    public int BusId { get; set; }
    public string BusName { get; set; } = string.Empty;
    public string BusRegNo { get; set; } = string.Empty;
    public string DriverName { get; set; } = string.Empty;
    public int CompanyId { get; set; }
    public int CategoryId { get; set; }

   // public BusCompanyEntity Company { get; set; } = null!;
  //  public BusCategory Category { get; set; } = null!;
    
}
