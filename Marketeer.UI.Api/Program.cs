using Marketeer.Common;
using Marketeer.Common.Configs;
using Marketeer.Core;
using Marketeer.Core.Service.Loggers;
using Marketeer.Infrastructure;
using Marketeer.Persistance;
using Marketeer.Persistance.Database.DbContexts;
using Marketeer.UI.Api;
using Microsoft.EntityFrameworkCore;
using Newtonsoft.Json;
using System.Security.Cryptography;

var builder = WebApplication.CreateBuilder(args);
var configuration = builder.Configuration;
var services = builder.Services;

builder.Logging.AddProvider(new DbLoggerProvider(configuration.GetConnectionString("DefaultConnection")!));

// Add services to the container.
services
    .AddHttpClient()
    .AddCommonServices(configuration)
    .AddPersistanceServices(configuration)
    .AddCoreServices(configuration)
    .AddInfrastructureServices(configuration)
    .AddApiServices(configuration);

services.AddControllers()
    .AddNewtonsoftJson(options =>
    {
        options.SerializerSettings.ReferenceLoopHandling = ReferenceLoopHandling.Serialize;
        options.SerializerSettings.PreserveReferencesHandling = PreserveReferencesHandling.Objects;
        options.SerializerSettings.TypeNameHandling = TypeNameHandling.None;
    });
services.AddCors();

var app = builder.Build();

// Migrate database
using (var scope = app.Services.CreateScope())
{
    scope.ServiceProvider
        .GetRequiredService<AppDbContext>()
        .Database
        .Migrate();
    scope.ClearCronJobStatuses();
    scope.SeedSecurityServiceScope();
    scope.SetupInfrastructureServices();
}

// Configure the HTTP request pipeline.

app.UseCors(x => x.AllowAnyOrigin()
    .AllowAnyMethod()
    .AllowAnyHeader());
app.UseHttpsRedirection();
app.UseAuthentication();
app.UseAuthorization();
app.MapControllers();

app.Run();
