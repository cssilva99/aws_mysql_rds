using System.ComponentModel.DataAnnotations;

namespace Stratio.Challenges.VehicleMaintenances.Database.Models;

public class Maintenance
{
    [Key]
    public int Id { get; set; }

    [Required]
    [MaxLength(500)]
    public string Description { get; set; }

    [Required]
    [MaxLength(50)]
    public string VehicleVin { get; set; }

    [Required]
    public DateTime ScheduledDate { get; set; }

    public DateTime? CompletedDate { get; set; }
    public int? Odometer { get; set; }
}
