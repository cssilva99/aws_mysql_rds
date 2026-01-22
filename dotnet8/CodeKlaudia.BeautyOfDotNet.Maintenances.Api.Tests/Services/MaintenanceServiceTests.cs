using Stratio.Challenges.VehicleMaintenances.Api.Services;
using Stratio.Challenges.VehicleMaintenances.Database.Models;

namespace Stratio.Challenges.VehicleMaintenances.Api.Tests.Services;

public class MaintenanceServiceTests
{
    private readonly MaintenanceService _maintenanceService = new();

    [Fact]
    public async Task TestsAddMaintenance()
    {
        var maintenance = new Maintenance
        {
            Description = "description",
            ScheduledDate = DateTime.UtcNow,
        };

        var added = await _maintenanceService.AddMaintenanceAsync(maintenance);

        Assert.Equal(maintenance, added);
    }
}
