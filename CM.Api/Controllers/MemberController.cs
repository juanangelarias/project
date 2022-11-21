using CM.Model.Dto;
using CM.Model.General;
using CM.Repositories;
using Microsoft.AspNetCore.Mvc;
using Serilog;

namespace CM.Api.Controllers;

//[Authorize]
[ApiController]
[Route("api/member/v1")]
public class MemberController : Controller
{
    private readonly IMemberRepository _memberRepository;

    public MemberController(IMemberRepository memberRepository)
    {
        _memberRepository = memberRepository;
    }

    [HttpGet]
    public async Task<ActionResult<IEnumerable<MemberDto>>> Get()
    {
        try
        {
            return Ok(await _memberRepository.GetAsync());
        }
        catch (Exception exception)
        {
            Log.Fatal(exception, "{Message}", exception.Message);
            return StatusCode(500);
        }
    }

    [HttpGet("{id:long}")]
    public async Task<ActionResult<MemberDto>> Get([FromRoute] long id)
    {
        try
        {
            return Ok(await _memberRepository.GetByIdAsync(id));
        }
        catch (Exception exception)
        {
            Log.Fatal(exception, "{Message}", exception.Message);
            return StatusCode(500);
        }
    }

    [HttpGet("getPage")]
    public async Task<ActionResult<PagedResponse<MemberDto>>> GetPage([FromQuery] QueryParams parameters)
    {
        try
        {
            return Ok(await _memberRepository.GetPageAsync(parameters));
        }
        catch (Exception exception)
        {
            Log.Fatal(exception, "{Message}", exception.Message);
            return StatusCode(500);
        }
    }

    [HttpGet("autocomplete")]
    public async Task<ActionResult<IEnumerable<MemberDto>>> Autocomplete([FromQuery] AutoCompleteParams parameters)
    {
        try
        {
            return Ok(await _memberRepository.Autocomplete(parameters));
        }
        catch (Exception exception)
        {
            Log.Fatal(exception, "{Message}", exception.Message);
            return StatusCode(500);
        }
    }

    [HttpPost]
    public async Task<ActionResult<MemberDto>> Create([FromBody] MemberDto member)
    {
        try
        {
            member.Club = null;
            return Ok(await _memberRepository.CreateAsync(member));
        }
        catch (Exception exception)
        {
            Log.Fatal(exception, "{Message}", exception.Message);
            return StatusCode(500);
        }
    }

    [HttpPut]
    public async Task<ActionResult<MemberDto>> Update([FromBody] MemberDto member)
    {
        try
        {
            return Ok(await _memberRepository.UpdateAsync(member));
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
            await _memberRepository.DeleteAsync(id);
            
            return NoContent();
        }
        catch (Exception exception)
        {
            Log.Fatal(exception, "{Message}", exception.Message);
            return StatusCode(500);
        }
    }
}