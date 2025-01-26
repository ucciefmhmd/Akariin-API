using API.Hubs;
using Configurations;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddStartupServices(builder.Configuration);
builder.Services.AddSignalR();

var app = builder.Build();

app.ConfigureApplication();

app.Run();