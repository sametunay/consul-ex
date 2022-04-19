using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;

namespace ConsulExample;

[ApiController]
[Route("/")]
public class BaseController : ControllerBase
{
    private readonly IConfiguration _config;

    public BaseController(IConfiguration config)
    {
        _config = config;
    }


    [HttpGet]
    [ApiExplorerSettings(IgnoreApi = true)]
    public IActionResult Info()
    {
        var app = _config.GetSection("app").Get<string>();
        var version = _config.GetSection("version").Get<string>();

        return Ok(new
        {
            app,
            version
        });
    }
}