using Microsoft.AspNetCore.Mvc;
using QueueInsight.Api.Models;
using QueueInsight.Api.Services;

namespace QueueInsight.Api.Controllers;

[ApiController]
[Route("api/[controller]")]
public class VhostsController : ControllerBase
{
    private readonly IRabbitMqService _rabbitMqService;
    private readonly ILogger<VhostsController> _logger;

    public VhostsController(IRabbitMqService rabbitMqService, ILogger<VhostsController> logger)
    {
        _rabbitMqService = rabbitMqService;
        _logger = logger;
    }

    [HttpGet]
    public async Task<ActionResult<List<VirtualHost>>> GetVirtualHosts()
    {
        try
        {
            var vhosts = await _rabbitMqService.GetVirtualHostsAsync();
            return Ok(vhosts);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error retrieving virtual hosts");
            return StatusCode(500, new { error = "Failed to retrieve virtual hosts", details = ex.Message });
        }
    }
}
