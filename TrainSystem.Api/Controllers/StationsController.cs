using Microsoft.AspNetCore.Mvc;
using TrainSystem.Api.Models;
using TrainSystem.Api.Repositories;

namespace TrainSystem.Api.Controllers;

// Read-only endpoints that show the generic repository being injected and used.
// Station management (create/update/delete) will be added with the Admin role in a later phase.
[ApiController]
[Route("api/[controller]")]
public class StationsController : ControllerBase
{
    private readonly IRepository<Station> _stationRepository;

    public StationsController(IRepository<Station> stationRepository)
    {
        _stationRepository = stationRepository;
    }

    [HttpGet]
    public async Task<ActionResult<List<Station>>> GetAll()
    {
        List<Station> stations = await _stationRepository.GetAllAsync();
        return Ok(stations);
    }

    [HttpGet("{id:int}")]
    public async Task<ActionResult<Station>> GetById(int id)
    {
        Station? station = await _stationRepository.GetByIdAsync(id);

        if (station == null)
        {
            return NotFound();
        }

        return Ok(station);
    }
}
