namespace ezyGo.Admin.Storage.Entities;

public class BusCategory
{
    public int Id { get; set; }
    public int Capacity { get; set; }
    public BusType Type { get; set; }
    public ICollection<Bus> Buses { get; set; } = new List<Bus>();
}

public enum BusType
{
    AC = 1,
    NONAC = 2,
}