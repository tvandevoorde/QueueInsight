using Microsoft.AspNetCore.Mvc;
using QueueInsight.Api.Models;
using QueueInsight.Api.Services;

namespace QueueInsight.Api.Controllers;

[ApiController]
[Route("api/[controller]")]
public class MessagesController : ControllerBase
{
    private readonly IRabbitMqService _rabbitMqService;
    private readonly ILogger<MessagesController> _logger;

    public MessagesController(IRabbitMqService rabbitMqService, ILogger<MessagesController> logger)
    {
        _rabbitMqService = rabbitMqService;
        _logger = logger;
    }

    [HttpPost("publish")]
    public async Task<ActionResult> PublishMessage([FromBody] PublishMessageRequest request)
    {
        try
        {
            await _rabbitMqService.PublishMessageAsync(request);
            return Ok(new { message = "Message published successfully" });
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error publishing message");
            return StatusCode(500, new { error = "Failed to publish message", details = ex.Message });
        }
    }

    [HttpDelete("delete")]
    public async Task<ActionResult> DeleteMessages([FromBody] DeleteMessageRequest request)
    {
        try
        {
            await _rabbitMqService.DeleteMessagesAsync(request);
            return Ok(new { message = "Messages deleted successfully" });
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error deleting messages");
            return StatusCode(500, new { error = "Failed to delete messages", details = ex.Message });
        }
    }

    [HttpPost("move")]
    public async Task<ActionResult> MoveMessages([FromBody] MoveMessageRequest request)
    {
        try
        {
            await _rabbitMqService.MoveMessagesAsync(request);
            return Ok(new { message = "Messages moved successfully" });
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error moving messages");
            return StatusCode(500, new { error = "Failed to move messages", details = ex.Message });
        }
    }
}
