namespace ezyGo.Admin.Domain.Models;

public class Route
{
    public int Id { get; set; }
    public Station? StartingPoint { get; set; }
    public Station? EndingingPoint { get; set; }
}

