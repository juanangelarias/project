using CM.Model.Dto;
using CM.Model.General;
using CM.Repositories;
using Microsoft.AspNetCore.Mvc;
using Serilog;

namespace CM.Api.Controllers;

//[Authorize]
[ApiController]
[Route("api/conference/v1")]
public class ConferenceController: Controller
{
    private readonly IConferenceRepository _conferenceRepository;

    public ConferenceController(IConferenceRepository conferenceRepository)
    {
        _conferenceRepository = conferenceRepository;
    }
    
    [HttpGet]
    public async Task<ActionResult<IEnumerable<ConferenceDto>>> Get()
    {
        try
        {
            return Ok(await _conferenceRepository.GetAllAsync());
        }
        catch (Exception exception)
        {
            Log.Fatal(exception, "{Message}", exception.Message);
            return StatusCode(500);
        }
    }

    [HttpGet("{id:long}")]
    public async Task<ActionResult<ConferenceDto>> Get([FromRoute] long id)
    {
        try
        {
            return Ok(await _conferenceRepository.GetByIdAsync(id));
        }
        catch (Exception exception)
        {
            Log.Fatal(exception, "{Message}", exception.Message);
            return StatusCode(500);
        }
    }

    [HttpGet("getPage")]
    public async Task<ActionResult<PagedResponse<ConferenceDto>>> GetPage(
        [FromQuery] QueryParams parameters
    )
    {
        try
        {
            return Ok(await _conferenceRepository.GetPageAsync(parameters));
        }
        catch (Exception exception)
        {
            Log.Fatal(exception, "{Message}", exception.Message);
            return StatusCode(500);
        }
    }

    [HttpGet("autocomplete")]
    public async Task<ActionResult<IEnumerable<ConferenceDto>>> Autocomplete(
        [FromQuery] AutoCompleteParams parameters
    )
    {
        try
        {
            return Ok(await _conferenceRepository.Autocomplete(parameters));
        }
        catch (Exception exception)
        {
            Log.Fatal(exception, "{Message}", exception.Message);
            return StatusCode(500);
        }
    }

    [HttpPost]
    public async Task<ActionResult<ConferenceDto>> Create([FromBody] ConferenceDto conference)
    {
        try
        {
            conference.HostClub = null;
            conference.PrimaryCurrency = null;
            conference.SecondaryCurrency = null;
            return Ok(await _conferenceRepository.CreateAsync(conference));
        }
        catch (Exception exception)
        {
            Log.Fatal(exception, "{Message}", exception.Message);
            return StatusCode(500);
        }
    }

    [HttpPut]
    public async Task<ActionResult<ConferenceDto>> Update([FromBody] ConferenceDto conference)
    {
        try
        {
            conference.HostClub = null;
            conference.PrimaryCurrency = null;
            conference.SecondaryCurrency = null;
            return Ok(await _conferenceRepository.UpdateAsync(conference));
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
            await _conferenceRepository.DeleteAsync(id);

            return NoContent();
        }
        catch (Exception exception)
        {
            Log.Fatal(exception, "{Message}", exception.Message);
            return StatusCode(500);
        }
    }
}