using System.ComponentModel.DataAnnotations;

namespace ezyGo.Admin.Domain.Models;

public class Route
{
    public int Id { get; set; }

    [Required]
    public Station StartingPoint { get; set; } = null!;

    [Required]
    public Station EndingPoint { get; set; } = null!;

    public IList<RouteStoppage> Stoppages { get; set; } = [];
}

