namespace QueueInsight.Api.Models;

public class RabbitMqSettings
{
    public string ManagementUrl { get; set; } = "http://localhost:15672";
    public string Username { get; set; } = "guest";
    public string Password { get; set; } = "guest";
}
