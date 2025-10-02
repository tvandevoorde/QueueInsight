using Microsoft.AspNetCore.Mvc;
using QueueInsight.Api.Models;
using QueueInsight.Api.Services;

namespace QueueInsight.Api.Controllers;

[ApiController]
[Route("api/[controller]")]
public class QueuesController : ControllerBase
{
    private readonly IRabbitMqService _rabbitMqService;
    private readonly ILogger<QueuesController> _logger;

    public QueuesController(IRabbitMqService rabbitMqService, ILogger<QueuesController> logger)
    {
        _rabbitMqService = rabbitMqService;
        _logger = logger;
    }

    [HttpGet("{vhost}")]
    public async Task<ActionResult<List<Queue>>> GetQueues(string vhost)
    {
        try
        {
            var queues = await _rabbitMqService.GetQueuesAsync(vhost);
            return Ok(queues);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error retrieving queues for vhost: {Vhost}", vhost);
            return StatusCode(500, new { error = "Failed to retrieve queues", details = ex.Message });
        }
    }

    [HttpGet("{vhost}/{queue}/messages")]
    public async Task<ActionResult<MessageResponse>> GetMessages(string vhost, string queue, [FromQuery] int count = 10)
    {
        try
        {
            var messages = await _rabbitMqService.GetMessagesAsync(vhost, queue, count);
            return Ok(messages);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error retrieving messages from queue: {Queue}", queue);
            return StatusCode(500, new { error = "Failed to retrieve messages", details = ex.Message });
        }
    }
}
