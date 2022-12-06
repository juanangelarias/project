using CM.Entities;
using CM.Features;
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
    private readonly IConferenceFeature _conferenceFeature;

    public ConferenceController(IConferenceRepository conferenceRepository, IConferenceFeature conferenceFeature)
    {
        _conferenceRepository = conferenceRepository;
        _conferenceFeature = conferenceFeature;
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

    [HttpGet("inscriptions")]
    public async Task<ActionResult<PagedResponse<InscriptionDto>>> GetInscriptionByConference(
        [FromQuery] InscriptionQueryParams parameters)
    {
        try
        {
            return Ok(await _conferenceFeature.GetInscriptionsByConference(parameters));
        }
        catch (Exception exception)
        {
            Log.Fatal(exception, "{Message}", exception.Message);
            return StatusCode(500);
        }
    }

    [HttpGet("{conferenceId:long}/products")]
    public async Task<ActionResult<IEnumerable<ProductDto>>> GetProductsByConference([FromRoute] long conferenceId)
    {
        try
        {
            return Ok(await _conferenceFeature.GetProductsByConference(conferenceId));
        }
        catch (Exception exception)
        {
            Log.Fatal(exception, "{Message}", exception.Message);
            return StatusCode(500);
        }
    }

    [HttpGet("{conferenceId:long}/programs")]
    public async Task<ActionResult<IEnumerable<ProgramDto>>> GetProgramsByConference([FromRoute] long conferenceId)
    {
        try
        {
            return Ok(await _conferenceFeature.GetProgramsByConference(conferenceId));
        }
        catch (Exception exception)
        {
            Log.Fatal(exception, "{Message}", exception.Message);
            return StatusCode(500);
        }
    }

    [HttpGet("{conferenceId:long}/paymentMethods")]
    public async Task<ActionResult<IEnumerable<ConferencePaymentMethodDto>>> GetPaymentMethodsByConference(
        [FromRoute] long conferenceId)
    {
        try
        {
            return Ok(await _conferenceFeature.GetPaymentMethodsByConference(conferenceId));
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
            return Ok(await _conferenceFeature.CreateAsync(conference));
        }
        catch (Exception exception)
        {
            Log.Fatal(exception, "{Message}", exception.Message);
            return StatusCode(500);
        }
    }

    [HttpPost("program")]
    public async Task<ActionResult<ProgramDto>> CreateProgram([FromBody] ProgramDto program)
    {
        try
        {
            return Ok(await _conferenceFeature.CreateProgramAsync(program));
        }
        catch (Exception exception)
        {
            Log.Fatal(exception, "{Message}", exception.Message);
            return StatusCode(500);
        }
    }

    [HttpPost("paymentMethods")]
    public async Task<ActionResult<IEnumerable<ConferencePaymentMethodDto>>> SetPaymentMethods(
        List<ConferencePaymentMethodDto> paymentMethods)
    {
        try
        {
            return Ok(await _conferenceFeature.SetPaymentMethodsByConference(paymentMethods));
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

    [HttpPut("program")]
    public async Task<ActionResult<ProgramDto>> UpdateProgram([FromBody] ProgramDto program)
    {
        try
        {
            return Ok(await _conferenceFeature.UpdateProgramAsync(program));
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

    [HttpDelete("program/{programId:long}")]
    public async Task<IActionResult> DeleteProgram([FromRoute] long programId)
    {
        try
        {
            await _conferenceFeature.DeleteProgramAsync(programId);

            return NoContent();
        }
        catch (Exception exception)
        {
            Log.Fatal(exception, "{Message}", exception.Message);
            return StatusCode(500);
        }
    }
}