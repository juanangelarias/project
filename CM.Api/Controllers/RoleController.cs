using CM.Model.Dto;
using CM.Model.General;
using CM.Repositories;
using Microsoft.AspNetCore.Mvc;
using Serilog;

namespace CM.Api.Controllers;

//[Authorize]
[ApiController]
[Route("api/role/v1")]
public class RoleController : ControllerBase
{
    private readonly IRoleRepository _roleRepository;

    public RoleController(IRoleRepository roleRepository)
    {
        _roleRepository = roleRepository;
    }

    [HttpGet]
    public async Task<ActionResult<IEnumerable<RoleDto>>> Get()
    {
        try
        {
            var list = await _roleRepository.GetAsync();

            return Ok(list);
        }
        catch (Exception exception)
        {
            Log.Fatal(exception, "{Message}", exception.Message);
            return StatusCode(500);
        }
    }

    [HttpGet("{roleId:long}")]
    public async Task<ActionResult<RoleDto>> Get([FromRoute] long roleId)
    {
        try
        {
            var role = await _roleRepository.GetByIdAsync(roleId);

            return Ok(role);
        }
        catch (Exception exception)
        {
            Log.Fatal(exception, "{Message}", exception.Message);
            return StatusCode(500);
        }
    }

    [HttpGet("getPage")]
    public async Task<ActionResult<PagedResponse<RoleDto>>> GetPage([FromQuery] QueryParams parameters)
    {
        try
        {
            var page = await _roleRepository.GetPageAsync(parameters);

            return Ok(page);
        }
        catch (Exception exception)
        {
            Log.Fatal(exception, "{Message}", exception.Message);
            return StatusCode(500);
        }
    }

    [HttpPost]
    public async Task<ActionResult<RoleDto>> Create([FromBody] RoleDto role)
    {
        try
        {
            var result = await _roleRepository.CreateAsync(role);

            return Ok(result);
        }
        catch (Exception exception)
        {
            Log.Fatal(exception, "{Message}", exception.Message);
            return StatusCode(500);
        }
    }

    [HttpPut]
    public async Task<ActionResult<RoleDto>> Update([FromBody] RoleDto role)
    {
        try
        {
            var result = await _roleRepository.UpdateAsync(role);

            return Ok(result);
        }
        catch (Exception exception)
        {
            Log.Fatal(exception, "{Message}", exception.Message);
            return StatusCode(500);
        }
    }

    [HttpDelete("{roleId:long}")]
    public async Task<IActionResult> Delete([FromRoute] long roleId)
    {
        try
        {
            await _roleRepository.DeleteAsync(roleId);

            return NoContent();
        }
        catch (Exception exception)
        {
            Log.Fatal(exception, "{Message}", exception.Message);
            return StatusCode(500);
        }
    }
}