namespace ezyGo.Admin.Domain.Models;

public class RouteStoppage
{
    public int Id { get; set; }
    public int RouteId { get; set; }
    public int StationId { get; set; }
    public int Order { get; set; }
}
