using Microsoft.EntityFrameworkCore;
using Stratio.Challenges.VehicleMaintenances.Database;
using Stratio.Challenges.VehicleMaintenances.Database.Models;

namespace Stratio.Challenges.VehicleMaintenances.Api.Services;

public class MaintenanceService : IMaintenanceService
{
    private readonly MaintenancesContext _maintenancesContext;

    public MaintenanceService(MaintenancesContext maintenancesContext)
    {
        _maintenancesContext = maintenancesContext;
    }

    public async Task<List<Maintenance>> GetAllAsync(string? vehicleVin = null)
    {
        var query = _maintenancesContext.Maintenances.AsQueryable();
        
        if (!string.IsNullOrEmpty(vehicleVin))
        {
            query = query.Where(m => m.VehicleVin == vehicleVin);
        }

        return await query.ToListAsync();
    }

    public async Task<Maintenance?> GetByIdAsync(int id) // Alterado para int
    {
        return await _maintenancesContext.Maintenances.FindAsync(id);
    }

    public async Task AddAsync(Maintenance maintenance)
    {
        await _maintenancesContext.Maintenances.AddAsync(maintenance);
        await _maintenancesContext.SaveChangesAsync();
    }

    public async Task UpdateAsync(Maintenance maintenance)
    {
        _maintenancesContext.Maintenances.Update(maintenance);
        await _maintenancesContext.SaveChangesAsync();
    }
}



