using Domain.Common;
using Receiver.Consumers;
using Receiver.Models;
using Serilog;

var builder = WebApplication.CreateBuilder(args);

builder.Host.UseSerilog((context, loggerConfig) => loggerConfig.ReadFrom.Configuration(context.Configuration));

builder.Services.Configure<RabbitmqSettings>(opt => builder.Configuration.GetSection("RabbitmqSettings").Bind(opt));
builder.Services.AddHostedService<HeroRabbitmqConsumer>();
builder.Services.AddHostedService<TextRabbitmqConsumer>();

builder.Services.Configure<BaseApiSettings>(opt => builder.Configuration.GetSection("BaseApiSettings").Bind(opt));
builder.Services.AddHttpClient<HeroRabbitmqConsumer>();

var app = builder.Build();

app.Run();
