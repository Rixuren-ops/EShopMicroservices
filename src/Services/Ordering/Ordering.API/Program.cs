using Ordering.API;
using Ordering.Application;
using Ordering.Infrastructure;
using Ordering.Infrastructure.Data.Extensions;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddApplicationServices(builder.Configuration)
    .AddInfraestructureServices(builder.Configuration)
    .AddApiServices(builder.Configuration);

var app = builder.Build();

app.UseApiServices();

if(app.Environment.IsDevelopment())
{
    await app.InitializeDatabaseAsync();
}

app.Run();
 