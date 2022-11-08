using CM.Model.Dto;
using CM.Model.General;
using CM.Repositories;
using Microsoft.AspNetCore.Mvc;
using Serilog;

namespace CM.Api.Controllers;

//[Authorize]
[ApiController]
[Route("api/user/v1")]
public class UserController: ControllerBase
{
    private readonly IUserRepository _userRepository;

    public UserController(IUserRepository userRepository)
    {
        _userRepository = userRepository;
    }

    [HttpGet]
    public async Task<ActionResult<IEnumerable<UserDto>>> Get()
    {
        try
        {
            return Ok(await _userRepository.GetAllAsync(true));
        }
        catch (Exception exception)
        {
            Log.Fatal(exception, "{Message}", exception.Message);
            return StatusCode(500);
        }
    }

    [HttpGet("{userId:long}")]
    public async Task<ActionResult<UserDto>> Get([FromRoute] long userId)
    {
        try
        {
            return Ok(await _userRepository.GetByIdExpandedAsync(userId));
        }
        catch (Exception exception)
        {
            Log.Fatal(exception, "{Message}", exception.Message);
            return StatusCode(500);
        }
    }

    [HttpGet("getPage")]
    public async Task<ActionResult<PagedResponse<UserDto>>> GetPage([FromQuery] QueryParams parameters)
    {
        try
        {
            return Ok(await _userRepository.GetAllPagedAsync(parameters, true));
        }
        catch (Exception exception)
        {
            Log.Fatal(exception, "{Message}", exception.Message);
            return StatusCode(500);
        }
    }

    [HttpPost]
    public async Task<ActionResult<UserDto>> Create([FromBody] UserDto user)
    {
        try
        {
            return Ok(await _userRepository.CreateAsync(user));
        }
        catch (Exception exception)
        {
            Log.Fatal(exception, "{Message}", exception.Message);
            return StatusCode(500);
        }
    }

    [HttpPut]
    public async Task<ActionResult<UserDto>> Update([FromBody] UserDto user)
    {
        try
        {
            return Ok(await _userRepository.UpdateAsync(user));
        }
        catch (Exception exception)
        {
            Log.Fatal(exception, "{Message}", exception.Message);
            return StatusCode(500);
        }
    }

    [HttpDelete("{userId:long}")]
    public async Task<IActionResult> Delete([FromRoute] long userId)
    {
        try
        {
            await _userRepository.DeleteAsync(userId);

            return NoContent();
        }
        catch (Exception exception)
        {
            Log.Fatal(exception, "{Message}", exception.Message);
            return StatusCode(500);
        }
    }
}