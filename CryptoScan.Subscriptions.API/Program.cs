using Carter;
using CryptoScan.Subscriptions.API;
using CryptoScan.Subscriptions.API.Extensions;

var builder = WebApplication.CreateBuilder(args);
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();
builder.Services.AddCarter();

var databaseConfig = builder.Configuration.GetSection("SubscriptionsDatabase").Get<DatabaseSettings>()!;
builder.Services.AddSubscriptions(databaseConfig);

var app = builder.Build();

app.UseSwagger();
app.MapCarter();

app.UseSwaggerUI();

app.Run();
app.Run();
