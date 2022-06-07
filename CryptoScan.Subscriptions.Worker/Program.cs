using CryptoScan.Subscriptions.Worker;
using CryptoScan.Subscriptions.Worker.Options;
using RabbitMQ.Client;

var builder = WebApplication.CreateBuilder(args);
builder.Logging.AddConsole();

var rabbitMqOptions = builder.Configuration.GetSection(RabbitMqOptions.SectionName).Get<RabbitMqOptions>()!;
Console.WriteLine($"Rabbit: {rabbitMqOptions.HostName}:{rabbitMqOptions.Port}");
builder.Services.AddSingleton<IConnectionFactory>(new ConnectionFactory
{
    HostName = rabbitMqOptions.HostName, 
    Port = rabbitMqOptions.Port, 
    UserName = rabbitMqOptions.UserName, 
    Password = rabbitMqOptions.Password
});

builder.Services.Configure<SubscriptionsApiOptions>(
    builder.Configuration.GetSection(SubscriptionsApiOptions.SectionName));
builder.Services.AddHttpClient();
builder.Services.AddSingleton<ISubscriptionService, SubscriptionService>();
builder.Services.AddHostedService<SubscriptionChangeMessageReceiver>();

var app = builder.Build();
app.Run();