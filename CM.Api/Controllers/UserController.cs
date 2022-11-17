using CM.Features;
using CM.Model.Dto;
using CM.Model.Enum;
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
    private readonly IUserFeature _userFeature;

    public UserController(IUserRepository userRepository, IUserFeature userFeature)
    {
        _userRepository = userRepository;
        _userFeature = userFeature;
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

    [HttpGet("{userId:long}/sendMail/{template:int}")]
    public async Task<ActionResult<bool>> SendMail([FromRoute] long userId, [FromRoute] int template)
    {
        try
        {
            var tpl = (EmailTemplate) template;
            await _userFeature.SendMail(userId, tpl);

            return Ok(true);
        }
        catch (Exception exception)
        {
            Log.Fatal(exception, "{Message}", exception.Message);
            return StatusCode(500);
        }
    }

    [HttpGet("getUserFromToken")]
    public async Task<ActionResult<PasswordMailData>> GetUserFromToken([FromQuery] string token)
    {
        try
        {
            return Ok(await _userFeature.GetUserFromToken(token));
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

    [HttpPost("login")]
    public async Task<ActionResult<LoginResponse>> Login([FromBody] LoginRequest data)
    {
        try
        {
            return Ok(await _userFeature.Login(data));
        }
        catch (Exception exception)
        {
            Log.Fatal(exception, "{Message}", exception.Message);
            return StatusCode(500);
        }
    }

    [HttpPost("changePassword")]
    public async Task<ActionResult<ChangePasswordResponse>> ChangePassword([FromBody] ChangePasswordRequest request)
    {
        try
        {
            var response = await _userFeature.ChangePassword(request);

            return Ok(response);
        }
        catch (Exception exception)
        {
            Log.Fatal(exception, "{Message}", exception.Message);
            return StatusCode(500);
        }
    }

    [HttpPost("resetPassword")]
    public async Task<ActionResult<bool>> ResetPassword([FromBody] ResetPassword data)
    {
        try
        {
            return Ok(await _userFeature.ResetPassword(data));
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