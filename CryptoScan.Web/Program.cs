using System.Text;
using System.Text.Json;
using RabbitMQ.Client;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddControllersWithViews();
builder.Services.AddSingleton(new ConnectionFactory
{
    HostName = builder.Configuration["RabbitMQ:HostName"], 
    Port = Convert.ToInt32(builder.Configuration["RabbitMQ:Port"]), 
    UserName = builder.Configuration["RabbitMQ:UserName"], 
    Password = builder.Configuration["RabbitMQ:Password"]
});

var app = builder.Build();


if (!app.Environment.IsDevelopment())
    app.UseHsts();

app.UseHttpsRedirection();
app.UseStaticFiles();
app.UseRouting();

app.MapControllerRoute(
    name: "default",
    pattern: "{controller}/{action=Index}/{id?}");

app.MapFallbackToFile("index.html");

app.MapPost("test", (HttpRequest request, ConnectionFactory factory) =>
{
    var subscription = new Subscription("test@gmail.com", "Bitcoin");
    var message = Encoding.UTF8.GetBytes(JsonSerializer.Serialize(subscription));
    
    using var connection = factory.CreateConnection();
    using var channel = connection.CreateModel();
    channel.QueueDeclare(
        queue: "subscriptionRequests",
        durable: false,
        exclusive: false,
        autoDelete: false,
        arguments: null);
    channel.BasicPublish(
        exchange: "", 
        routingKey: "subscriptionRequests", 
        basicProperties: null,
        body: message);
    return "Message queued";
});

app.Run();


internal record Subscription(string Email, string Cryptocurrency);