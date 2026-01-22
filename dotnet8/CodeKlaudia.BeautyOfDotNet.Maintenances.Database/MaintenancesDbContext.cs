using Microsoft.EntityFrameworkCore;
using Stratio.Challenges.VehicleMaintenances.Database.Models;

namespace Stratio.Challenges.VehicleMaintenances.Database;

public class MaintenancesContext : DbContext
{
    public DbSet<Maintenance> Maintenances { get; set; }

    public MaintenancesContext(DbContextOptions<MaintenancesContext> options) : base(options)
    {
    }
} // <--- VÊ SE ESTA CHAVETA ESTÁ AQUI!