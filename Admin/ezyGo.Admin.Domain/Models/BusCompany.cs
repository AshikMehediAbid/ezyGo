namespace ezyGo.Admin.Domain.Models;

public class BusCompany
{
    public int CompanyId { get; set; }
    public string CompanyName { get; set; } = string.Empty;
    public string OwnerName { get; set; } = string.Empty;
    public string RegistrationNo { get; set; } = string.Empty;
    public DateTime CreatedAt { get; set; }
}
