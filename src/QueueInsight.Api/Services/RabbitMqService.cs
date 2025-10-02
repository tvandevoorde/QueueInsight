using System.Net.Http.Headers;
using System.Text;
using System.Text.Json;
using QueueInsight.Api.Models;

namespace QueueInsight.Api.Services;

public interface IRabbitMqService
{
    Task<List<VirtualHost>> GetVirtualHostsAsync();
    Task<List<Queue>> GetQueuesAsync(string vhost);
    Task<MessageResponse> GetMessagesAsync(string vhost, string queue, int count = 10);
    Task PublishMessageAsync(PublishMessageRequest request);
    Task DeleteMessagesAsync(DeleteMessageRequest request);
    Task MoveMessagesAsync(MoveMessageRequest request);
}

public class RabbitMqService : IRabbitMqService
{
    private readonly HttpClient _httpClient;
    private readonly RabbitMqSettings _settings;
    private readonly ILogger<RabbitMqService> _logger;

    public RabbitMqService(HttpClient httpClient, RabbitMqSettings settings, ILogger<RabbitMqService> logger)
    {
        _httpClient = httpClient;
        _settings = settings;
        _logger = logger;

        // Set up basic authentication
        var authToken = Convert.ToBase64String(Encoding.ASCII.GetBytes($"{_settings.Username}:{_settings.Password}"));
        _httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Basic", authToken);
        _httpClient.BaseAddress = new Uri(_settings.ManagementUrl);
    }

    public async Task<List<VirtualHost>> GetVirtualHostsAsync()
    {
        try
        {
            var response = await _httpClient.GetAsync("/api/vhosts");
            response.EnsureSuccessStatusCode();
            
            var content = await response.Content.ReadAsStringAsync();
            var vhosts = JsonSerializer.Deserialize<List<JsonElement>>(content);
            
            return vhosts?.Select(v => new VirtualHost 
            { 
                Name = v.GetProperty("name").GetString() ?? string.Empty 
            }).ToList() ?? new List<VirtualHost>();
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error fetching virtual hosts");
            throw;
        }
    }

    public async Task<List<Queue>> GetQueuesAsync(string vhost)
    {
        try
        {
            var encodedVhost = Uri.EscapeDataString(vhost);
            var response = await _httpClient.GetAsync($"/api/queues/{encodedVhost}");
            response.EnsureSuccessStatusCode();
            
            var content = await response.Content.ReadAsStringAsync();
            var options = new JsonSerializerOptions { PropertyNameCaseInsensitive = true };
            var queues = JsonSerializer.Deserialize<List<JsonElement>>(content, options);
            
            return queues?.Select(q => new Queue
            {
                Name = q.GetProperty("name").GetString() ?? string.Empty,
                Vhost = q.GetProperty("vhost").GetString() ?? string.Empty,
                Messages = q.TryGetProperty("messages", out var msgs) ? msgs.GetInt32() : 0,
                MessagesReady = q.TryGetProperty("messages_ready", out var ready) ? ready.GetInt32() : 0,
                MessagesUnacknowledged = q.TryGetProperty("messages_unacknowledged", out var unack) ? unack.GetInt32() : 0,
                State = q.TryGetProperty("state", out var state) ? state.GetString() ?? string.Empty : string.Empty
            }).ToList() ?? new List<Queue>();
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error fetching queues for vhost: {Vhost}", vhost);
            throw;
        }
    }

    public async Task<MessageResponse> GetMessagesAsync(string vhost, string queue, int count = 10)
    {
        try
        {
            var encodedVhost = Uri.EscapeDataString(vhost);
            var encodedQueue = Uri.EscapeDataString(queue);
            
            var requestBody = new
            {
                count = count,
                ackmode = "ack_requeue_false",
                encoding = "auto"
            };
            
            var content = new StringContent(
                JsonSerializer.Serialize(requestBody),
                Encoding.UTF8,
                "application/json");
            
            var response = await _httpClient.PostAsync(
                $"/api/queues/{encodedVhost}/{encodedQueue}/get",
                content);
            response.EnsureSuccessStatusCode();
            
            var responseContent = await response.Content.ReadAsStringAsync();
            var options = new JsonSerializerOptions { PropertyNameCaseInsensitive = true };
            var messages = JsonSerializer.Deserialize<List<JsonElement>>(responseContent, options) ?? new List<JsonElement>();
            
            var result = new MessageResponse
            {
                MessageCount = messages.Count,
                Messages = messages.Select(m => new Message
                {
                    Payload = m.TryGetProperty("payload", out var payload) ? payload.GetString() ?? string.Empty : string.Empty,
                    PayloadEncoding = m.TryGetProperty("payload_encoding", out var encoding) ? encoding.GetString() ?? "string" : "string",
                    PayloadBytes = m.TryGetProperty("payload_bytes", out var bytes) ? bytes.GetInt32() : 0,
                    Redelivered = m.TryGetProperty("redelivered", out var redelivered) && redelivered.GetBoolean(),
                    Exchange = m.TryGetProperty("exchange", out var exchange) ? exchange.GetString() : null,
                    RoutingKey = m.TryGetProperty("routing_key", out var routingKey) ? routingKey.GetString() : null,
                    Properties = m.TryGetProperty("properties", out var props) 
                        ? JsonSerializer.Deserialize<Dictionary<string, object>>(props.GetRawText()) 
                        : null
                }).ToList()
            };
            
            return result;
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error fetching messages from queue: {Queue} in vhost: {Vhost}", queue, vhost);
            throw;
        }
    }

    public async Task PublishMessageAsync(PublishMessageRequest request)
    {
        try
        {
            var encodedVhost = Uri.EscapeDataString(request.Vhost);
            var encodedQueue = Uri.EscapeDataString(request.Queue);
            
            var requestBody = new
            {
                properties = request.Properties ?? new Dictionary<string, object>(),
                routing_key = request.Queue,
                payload = request.Payload,
                payload_encoding = request.PayloadEncoding
            };
            
            var content = new StringContent(
                JsonSerializer.Serialize(requestBody),
                Encoding.UTF8,
                "application/json");
            
            var response = await _httpClient.PostAsync(
                $"/api/exchanges/{encodedVhost}/amq.default/publish",
                content);
            response.EnsureSuccessStatusCode();
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error publishing message to queue: {Queue} in vhost: {Vhost}", request.Queue, request.Vhost);
            throw;
        }
    }

    public async Task DeleteMessagesAsync(DeleteMessageRequest request)
    {
        try
        {
            var encodedVhost = Uri.EscapeDataString(request.Vhost);
            var encodedQueue = Uri.EscapeDataString(request.Queue);
            
            // Get messages with ack to remove them
            var requestBody = new
            {
                count = request.Count,
                ackmode = "ack_requeue_false",
                encoding = "auto"
            };
            
            var content = new StringContent(
                JsonSerializer.Serialize(requestBody),
                Encoding.UTF8,
                "application/json");
            
            var response = await _httpClient.PostAsync(
                $"/api/queues/{encodedVhost}/{encodedQueue}/get",
                content);
            response.EnsureSuccessStatusCode();
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error deleting messages from queue: {Queue} in vhost: {Vhost}", request.Queue, request.Vhost);
            throw;
        }
    }

    public async Task MoveMessagesAsync(MoveMessageRequest request)
    {
        try
        {
            // First, get messages from source queue
            var messages = await GetMessagesAsync(request.SourceVhost, request.SourceQueue, request.Count);
            
            // Then publish them to destination queue
            foreach (var message in messages.Messages)
            {
                var publishRequest = new PublishMessageRequest
                {
                    Vhost = request.DestinationVhost,
                    Queue = request.DestinationQueue,
                    Payload = message.Payload,
                    PayloadEncoding = message.PayloadEncoding,
                    Properties = message.Properties
                };
                
                await PublishMessageAsync(publishRequest);
            }
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error moving messages from {SourceQueue} to {DestQueue}", request.SourceQueue, request.DestinationQueue);
            throw;
        }
    }
}
