using QueueInsight.Api.Models;
using QueueInsight.Api.Services;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddControllers();
builder.Services.AddOpenApi();

// Configure RabbitMQ settings from configuration
var rabbitMqSettings = new RabbitMqSettings();
builder.Configuration.GetSection("RabbitMq").Bind(rabbitMqSettings);
builder.Services.AddSingleton(rabbitMqSettings);

// Register HttpClient and RabbitMqService
builder.Services.AddHttpClient<IRabbitMqService, RabbitMqService>();

// Configure CORS
builder.Services.AddCors(options =>
{
    options.AddPolicy("AllowAngular", policy =>
    {
        policy.WithOrigins("http://localhost:4200")
              .AllowAnyHeader()
              .AllowAnyMethod();
    });
});

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.MapOpenApi();
}

app.UseHttpsRedirection();
app.UseCors("AllowAngular");
app.MapControllers();

app.Run();
