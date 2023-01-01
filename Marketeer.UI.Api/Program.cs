using Marketeer.Common;
using Marketeer.Core;
using Marketeer.Core.Service.Loggers;
using Marketeer.Persistance;
using Marketeer.Persistance.Database.DbContexts;
using Marketeer.UI.Api;
using Microsoft.EntityFrameworkCore;
using Newtonsoft.Json;

var builder = WebApplication.CreateBuilder(args);
var configuration = builder.Configuration;
var services = builder.Services;

builder.Logging.AddProvider(new DbLoggerProvider(configuration.GetConnectionString("DefaultConnection")!));

// Add services to the container.
services.AddCommonServices(configuration)
    .AddPersistanceServices(configuration)
    .AddCoreServices(configuration)
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
    scope.SeedSecurityServiceScope();
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
