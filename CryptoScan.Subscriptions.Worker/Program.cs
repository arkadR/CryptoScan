using CryptoScan.Subscriptions.Worker;
using RabbitMQ.Client;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddSingleton<IConnectionFactory>(new ConnectionFactory
{
    HostName = builder.Configuration["RabbitMQ:HostName"], 
    Port = Convert.ToInt32(builder.Configuration["RabbitMQ:Port"]), 
    UserName = builder.Configuration["RabbitMQ:UserName"], 
    Password = builder.Configuration["RabbitMQ:Password"]
});

var uri = new Uri(builder.Configuration["SubscriptionsApi:BaseUrl"]!);
builder.Services.AddHttpClient<ISubscriptionService, SubscriptionService>(client =>
{
    client.BaseAddress = uri;
});
builder.Services.AddSingleton<ISubscriptionService, SubscriptionService>();
builder.Services.AddHostedService<SubscriptionChangeMessageReceiver>();

var app = builder.Build();
app.Run();