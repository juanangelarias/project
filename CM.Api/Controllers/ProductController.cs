using CM.Features;
using CM.Model.Dto;
using CM.Model.General;
using CM.Repositories;
using Microsoft.AspNetCore.Mvc;
using Serilog;

namespace CM.Api.Controllers;

//[Authorize]
[ApiController]
[Route("api/product/v1")]
public class ProductController: Controller
{
    private readonly IProductFeature _productFeature;
    private readonly IProductRepository _productRepository;

    public ProductController(IProductFeature productFeature, IProductRepository productRepository)
    {
        _productFeature = productFeature;
        _productRepository = productRepository;
    }
    
    [HttpGet]
    public async Task<ActionResult<IEnumerable<ProductDto>>> Get()
    {
        try
        {
            return Ok(await _productRepository.GetAllAsync());
        }
        catch (Exception exception)
        {
            Log.Fatal(exception, "{Message}", exception.Message);
            return StatusCode(500);
        }
    }

    [HttpGet("{id:long}")]
    public async Task<ActionResult<ProductDto>> Get([FromRoute] long id)
    {
        try
        {
            return Ok(await _productRepository.GetByIdAsync(id));
        }
        catch (Exception exception)
        {
            Log.Fatal(exception, "{Message}", exception.Message);
            return StatusCode(500);
        }
    }

    [HttpGet("getPage")]
    public async Task<ActionResult<PagedResponse<ProductDto>>> GetPage(
        [FromQuery] QueryParams parameters
    )
    {
        try
        {
            return Ok(await _productRepository.GetPageAsync(parameters));
        }
        catch (Exception exception)
        {
            Log.Fatal(exception, "{Message}", exception.Message);
            return StatusCode(500);
        }
    }

    [HttpGet("{productId:long}/getChildren")]
    public async Task<ActionResult<IEnumerable<ProductDto>>> GetChildren([FromRoute] long productId)
    {
        try
        {
            return Ok(await _productFeature.GetChildren(productId));
        }
        catch (Exception exception)
        {
            Log.Fatal(exception, "{Message}", exception.Message);
            return StatusCode(500);
        }
    }

    [HttpGet("autocomplete")]
    public async Task<ActionResult<IEnumerable<ProductDto>>> Autocomplete(
        [FromQuery] AutoCompleteParams parameters
    )
    {
        try
        {
            return Ok(await _productRepository.Autocomplete(parameters));
        }
        catch (Exception exception)
        {
            Log.Fatal(exception, "{Message}", exception.Message);
            return StatusCode(500);
        }
    }

    [HttpPost]
    public async Task<ActionResult<ProductDto>> Create([FromBody] ProductDto club)
    {
        try
        {
            return Ok(await _productRepository.CreateAsync(club));
        }
        catch (Exception exception)
        {
            Log.Fatal(exception, "{Message}", exception.Message);
            return StatusCode(500);
        }
    }

    [HttpPut]
    public async Task<ActionResult<ProductDto>> Update([FromBody] ProductDto club)
    {
        try
        {
            return Ok(await _productRepository.UpdateAsync(club));
        }
        catch (Exception exception)
        {
            Log.Fatal(exception, "{Message}", exception.Message);
            return StatusCode(500);
        }
    }

    [HttpPut("{productId:long}")]
    public async Task<ActionResult<IEnumerable<ProductDto>>> UpdateChildren([FromRoute] long productId,
        [FromBody] List<ProductDto> children)
    {
        try
        {
            return Ok(await _productFeature.UpdateChildren(productId, children));
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
            await _productRepository.DeleteAsync(id);

            return NoContent();
        }
        catch (Exception exception)
        {
            Log.Fatal(exception, "{Message}", exception.Message);
            return StatusCode(500);
        }
    }
}