using ezyGo.Admin.Storage.Entities;

namespace ezyGo.Admin.Domain.Models;

public class BusCompany
{
    public int Id { get; set; }
    public string CompanyName { get; set; } = string.Empty;
    public string OwnerName { get; set; } = string.Empty;
    public string RegistrationNo { get; set; } = string.Empty;
    public DateTime CreatedAt { get; set; }
    public DateTime UpdatedAt { get; set; }
    public bool IsActive { get; set; } = true;

    public ICollection<Bus> Buses { get; set; } = new List<Bus>();
}
