using Stratio.Challenges.VehicleMaintenances.Database.Models;

namespace Stratio.Challenges.VehicleMaintenances.Api.Services;

public interface IMaintenanceService
{
    Task<List<Maintenance>> GetAllAsync(string? vehicleVin = null);
    Task<Maintenance?> GetByIdAsync(int id); // Alterado para int
    Task AddAsync(Maintenance maintenance);
    Task UpdateAsync(Maintenance maintenance);
}