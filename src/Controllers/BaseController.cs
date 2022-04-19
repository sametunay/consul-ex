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
    public IActionResult HealthCheck()
    {
        var email = _config.GetSection("email").Get<string>();

        return Ok(new
        {
            apiInfo = "consul-test-api",
            value = email
        });
    }
}