namespace QueueInsight.Api.Models;

public class Message
{
    public string Payload { get; set; } = string.Empty;
    public string PayloadEncoding { get; set; } = "string";
    public Dictionary<string, object>? Properties { get; set; }
    public string? RoutingKey { get; set; }
    public int PayloadBytes { get; set; }
    public bool Redelivered { get; set; }
    public string? Exchange { get; set; }
}

public class MessageResponse
{
    public List<Message> Messages { get; set; } = new();
    public int MessageCount { get; set; }
}

public class PublishMessageRequest
{
    public string Vhost { get; set; } = string.Empty;
    public string Queue { get; set; } = string.Empty;
    public string Payload { get; set; } = string.Empty;
    public string PayloadEncoding { get; set; } = "string";
    public Dictionary<string, object>? Properties { get; set; }
}

public class DeleteMessageRequest
{
    public string Vhost { get; set; } = string.Empty;
    public string Queue { get; set; } = string.Empty;
    public int Count { get; set; } = 1;
}

public class MoveMessageRequest
{
    public string SourceVhost { get; set; } = string.Empty;
    public string SourceQueue { get; set; } = string.Empty;
    public string DestinationVhost { get; set; } = string.Empty;
    public string DestinationQueue { get; set; } = string.Empty;
    public int Count { get; set; } = 1;
}
