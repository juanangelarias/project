using CM.Model.Dto;
using CM.Model.General;
using CM.Repositories;
using Microsoft.AspNetCore.Mvc;
using Serilog;

namespace CM.Api.Controllers;

//[Authorize]
[ApiController]
[Route("api/clubType/v1")]
public class ClubTypeController : Controller
{
    private readonly IClubTypeRepository _clubTypeRepository;

    public ClubTypeController(IClubTypeRepository clubTypeRepository)
    {
        _clubTypeRepository = clubTypeRepository;
    }

    [HttpGet]
    public async Task<ActionResult<IEnumerable<ClubTypeDto>>> Get()
    {
        try
        {
            return Ok(await _clubTypeRepository.GetAllAsync());
        }
        catch (Exception exception)
        {
            Log.Fatal(exception, "{Message}", exception.Message);
            return StatusCode(500);
        }
    }

    [HttpGet("{id:long}")]
    public async Task<ActionResult<ClubTypeDto>> Get([FromRoute] long id)
    {
        try
        {
            return Ok(await _clubTypeRepository.GetByIdAsync(id));
        }
        catch (Exception exception)
        {
            Log.Fatal(exception, "{Message}", exception.Message);
            return StatusCode(500);
        }
    }

    [HttpGet("getPage")]
    public async Task<ActionResult<PagedResponse<ClubTypeDto>>> GetPage(
        [FromQuery] QueryParams parameters
    )
    {
        try
        {
            return Ok(await _clubTypeRepository.GetPageAsync(parameters));
        }
        catch (Exception exception)
        {
            Log.Fatal(exception, "{Message}", exception.Message);
            return StatusCode(500);
        }
    }

    [HttpGet("autocomplete")]
    public async Task<ActionResult<IEnumerable<ClubTypeDto>>> Autocomplete(
        [FromQuery] AutoCompleteParams parameters
    )
    {
        try
        {
            return Ok(await _clubTypeRepository.Autocomplete(parameters));
        }
        catch (Exception exception)
        {
            Log.Fatal(exception, "{Message}", exception.Message);
            return StatusCode(500);
        }
    }

    [HttpPost]
    public async Task<ActionResult<ClubTypeDto>> Create([FromBody] ClubTypeDto club)
    {
        try
        {
            return Ok(await _clubTypeRepository.CreateAsync(club));
        }
        catch (Exception exception)
        {
            Log.Fatal(exception, "{Message}", exception.Message);
            return StatusCode(500);
        }
    }

    [HttpPut]
    public async Task<ActionResult<ClubTypeDto>> Update([FromBody] ClubTypeDto club)
    {
        try
        {
            return Ok(await _clubTypeRepository.UpdateAsync(club));
        }
        catch (Exception exception)
        {
            Log.Fatal(exception, "{Message}", exception.Message);
            return StatusCode(500);
        }
    }

    [HttpDelete("{id:long}")]
    public async Task<IActionResult> Delete([FromRoute] long id)
    {
        try
        {
            await _clubTypeRepository.DeleteAsync(id);

            return NoContent();
        }
        catch (Exception exception)
        {
            Log.Fatal(exception, "{Message}", exception.Message);
            return StatusCode(500);
        }
    }
}
