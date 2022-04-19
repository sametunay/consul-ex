using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;

namespace ConsulExample;

[ApiController]
[Route("[controller]")]
public class ConsulController : ControllerBase
{
    private readonly IConsulService _consulService;

    public ConsulController(IConsulService consulService)
    {
        _consulService = consulService;
    }

    [HttpGet]
    public async Task<IActionResult> GetKeys()
    {
        return Ok(await _consulService.GetKeysAsync());
    }

    [HttpGet("{key}")]
    public async Task<IActionResult> Get(string key)
    {
        return Ok(await _consulService.GetValueAsync(key));
    }

    [HttpPatch("{key}")]
    public async Task<IActionResult> Update(string key, [FromBody] object value)
    {
        await _consulService.UpdateAsync(key, value);
        return Ok();
    }

    [HttpPost("{key}")]
    public async Task<IActionResult> Create(string key, [FromQuery] object value)
    {
        await _consulService.CreateAsync(key, value);
        return Ok();
    }

    [HttpDelete("{key}")]
    public async Task<IActionResult> Delete(string key, [FromQuery] object value)
    {
        await _consulService.DeleteKeyAsync(key);
        return NoContent();
    }
}