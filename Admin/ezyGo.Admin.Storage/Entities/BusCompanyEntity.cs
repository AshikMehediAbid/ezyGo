using System.ComponentModel.DataAnnotations;

namespace ezyGo.Admin.Storage.Entities;

public class BusCompanyEntity
{
    [Key]
    public int Id { get; set; }

    [Required]
    [MaxLength(100)]
    public string CompanyName { get; set; } = string.Empty;

    [Required]
    [MaxLength(100)]
    public string OwnerName { get; set; } = string.Empty;

    [Required]
    [MaxLength(50)]
    public string RegistrationNo { get; set; } = string.Empty;

    public DateTime CreatedAt { get; set; } = DateTime.Now;

    public ICollection<BusEntity> Buses { get; set; } = new List<BusEntity>();

    public DateTime UpdatedAt { get; set; }
    public bool IsActive { get; set; } = true;


}
