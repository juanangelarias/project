using CM.Model.Dto;
using CM.Model.General;
using CM.Repositories;
using Microsoft.AspNetCore.Mvc;
using Serilog;

namespace CM.Api.Controllers;

//[Authorize]
[ApiController]
[Route("api/currency/v1")]
public class CurrencyController: Controller
{
    private readonly ICurrencyRepository _currencyRepository;

    public CurrencyController(ICurrencyRepository currencyRepository)
    {
        _currencyRepository = currencyRepository;
    }
    
    [HttpGet]
    public async Task<ActionResult<IEnumerable<CurrencyDto>>> Get()
    {
        try
        {
            return Ok(await _currencyRepository.GetAllAsync());
        }
        catch (Exception exception)
        {
            Log.Fatal(exception, "{Message}", exception.Message);
            return StatusCode(500);
        }
    }

    [HttpGet("{id:long}")]
    public async Task<ActionResult<CurrencyDto>> Get([FromRoute] long id)
    {
        try
        {
            return Ok(await _currencyRepository.GetByIdAsync(id));
        }
        catch (Exception exception)
        {
            Log.Fatal(exception, "{Message}", exception.Message);
            return StatusCode(500);
        }
    }

    [HttpGet("getPage")]
    public async Task<ActionResult<PagedResponse<CurrencyDto>>> GetPage(
        [FromQuery] QueryParams parameters
    )
    {
        try
        {
            return Ok(await _currencyRepository.GetPageAsync(parameters));
        }
        catch (Exception exception)
        {
            Log.Fatal(exception, "{Message}", exception.Message);
            return StatusCode(500);
        }
    }

    [HttpGet("autocomplete")]
    public async Task<ActionResult<IEnumerable<CurrencyDto>>> Autocomplete(
        [FromQuery] AutoCompleteParams parameters
    )
    {
        try
        {
            return Ok(await _currencyRepository.Autocomplete(parameters));
        }
        catch (Exception exception)
        {
            Log.Fatal(exception, "{Message}", exception.Message);
            return StatusCode(500);
        }
    }

    [HttpPost]
    public async Task<ActionResult<CurrencyDto>> Create([FromBody] CurrencyDto club)
    {
        try
        {
            return Ok(await _currencyRepository.CreateAsync(club));
        }
        catch (Exception exception)
        {
            Log.Fatal(exception, "{Message}", exception.Message);
            return StatusCode(500);
        }
    }

    [HttpPut]
    public async Task<ActionResult<CurrencyDto>> Update([FromBody] CurrencyDto club)
    {
        try
        {
            return Ok(await _currencyRepository.UpdateAsync(club));
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
            await _currencyRepository.DeleteAsync(id);

            return NoContent();
        }
        catch (Exception exception)
        {
            Log.Fatal(exception, "{Message}", exception.Message);
            return StatusCode(500);
        }
    }
}