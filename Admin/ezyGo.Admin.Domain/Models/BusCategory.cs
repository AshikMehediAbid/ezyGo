namespace ezyGo.Admin.Domain.Models;

public class BusCategory
{
    public int CategoryId { get; set; }
    public int Capacity { get; set; }
    public string Type { get; set; } = string.Empty;
}