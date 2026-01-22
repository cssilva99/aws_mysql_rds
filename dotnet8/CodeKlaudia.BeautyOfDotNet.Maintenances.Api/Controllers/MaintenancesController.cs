using Microsoft.AspNetCore.Mvc;
using Stratio.Challenges.VehicleMaintenances.Api.Services;
using Stratio.Challenges.VehicleMaintenances.Database.Models;

namespace Stratio.Challenges.VehicleMaintenances.Api.Controllers;

[ApiController]
[Route("[controller]")]
public class MaintenancesController : ControllerBase
{
    private readonly IMaintenanceService _service;

    public MaintenancesController(IMaintenanceService service)
    {
        _service = service;
    }

[HttpPost("ScheduleMaintenance")]
public async Task<ActionResult<Maintenance>> Schedule([FromBody] Maintenance maintenance)
    {
        if (maintenance.Odometer.HasValue && maintenance.Odometer < 0)
        {
            return BadRequest("Odometer value cannot be negative.");
        }

   
        if (string.IsNullOrEmpty(maintenance.VehicleVin) || string.IsNullOrEmpty(maintenance.Description))
        {
            return BadRequest("VIN and Description are required.");
        }

        await _service.AddAsync(maintenance);
    
    return CreatedAtAction("Get", new { id = maintenance.Id }, maintenance);
    }

    [HttpGet("GetMaintenances")]
    public async Task<IEnumerable<Maintenance>> Get([FromQuery] string? vehicleVin)
    {
        return await _service.GetAllAsync(vehicleVin);
    }

    [HttpGet("GetMaintenance/{id}")]
    public async Task<ActionResult<Maintenance>> Get(int id) // Usando int conforme o modelo
    {
        var maintenance = await _service.GetByIdAsync(id);

        if (maintenance == null)
        {
            return NotFound($"Maintenance with ID {id} not found.");
        }

        return Ok(maintenance);
    }

    [HttpPatch("CompleteMaintenance/{id}")]
    public async Task<IActionResult> Complete(int id, [FromQuery] int odometerValue) // Usando int
    {
        if (odometerValue < 0)
        {
            return BadRequest("Odometer value cannot be negative.");
        }

        var maintenance = await _service.GetByIdAsync(id);

        if (maintenance == null)
        {
            return NotFound($"Maintenance with ID {id} not found.");
        }

        if (maintenance.CompletedDate.HasValue)
        {
            return BadRequest("This maintenance intervention is already completed.");
        }

        maintenance.CompletedDate = DateTime.UtcNow;
        maintenance.Odometer = odometerValue;

        await _service.UpdateAsync(maintenance);
        return NoContent();
    }
}