using CleanArch.EntraApi.Application;
using CleanArch.EntraApi.Infrastructure;
using CleanArch.EntraApi.Infrastructure.Extensions;

var builder = WebApplication.CreateBuilder(args);

// Add services
builder.Services.AddApplication();
builder.Services.AddInfrastructure(builder.Configuration);
builder.Services.AddWebApiServices(builder.Configuration);

var app = builder.Build();

// Configure middleware pipeline
app.ConfigureWebApiPipeline(app.Environment, builder.Configuration);

app.Run();