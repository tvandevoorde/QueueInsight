namespace QueueInsight.Api.Models;

public class Queue
{
    public string Name { get; set; } = string.Empty;
    public string Vhost { get; set; } = string.Empty;
    public int Messages { get; set; }
    public int MessagesReady { get; set; }
    public int MessagesUnacknowledged { get; set; }
    public string State { get; set; } = string.Empty;
}
