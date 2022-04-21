using CryptoScan.Web.Main.Common;
using Microsoft.AspNetCore.Mvc;
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
builder.Services.AddHttpClient();
builder.Services.AddScoped<IHttp, Http>();
builder.Services.AddResponseCaching();
builder.Services.AddControllers(options =>
{
  options.CacheProfiles.Add("2h",
      new CacheProfile()
      {
        Duration = 2*60*60
      });
});

var app = builder.Build();

if (!app.Environment.IsDevelopment())
    app.UseHsts();

app.UseHttpsRedirection();
app.UseStaticFiles();
app.UseRouting();
app.UseResponseCaching();

app.MapControllerRoute(
    name: "default",
    pattern: "{controller}/{action=Index}/{id?}");

app.MapFallbackToFile("index.html");

app.Run();
