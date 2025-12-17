using Restaurants.Application.Extensions;
using Restaurants.Infrastructure.Extensions;
using Restaurants.Infrastructure.Seeders;
using Serilog;
using Serilog.Events;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers();
builder.Services.AddSwaggerGen();
builder.Services.AddInfrastructure(builder.Configuration);
builder.Services.AddApplication();

builder.Host.UseSerilog((_, configuration) =>
{
    configuration
        .MinimumLevel.Information() 
        .MinimumLevel.Override("Microsoft.EntityFrameworkCore", LogEventLevel.Information) 
        .Enrich.FromLogContext()
        .WriteTo.File("logs/Restaurant-API-.log", rollingInterval: RollingInterval.Day)
        .WriteTo.Logger(consoleLogger =>
        {
            consoleLogger
                .Filter.ByExcluding(logEvent =>
                {
                    if (logEvent.Level == LogEventLevel.Information)
                    {
                        if (logEvent.Properties.TryGetValue("SourceContext", out var value))
                        {
                            return value.ToString().Contains("Microsoft.EntityFrameworkCore");
                        }
                    }
                    return false;
                })
                .WriteTo.Console();
        });
});


var app = builder.Build();

var scope = app.Services.CreateScope();
var seeder = scope.ServiceProvider.GetRequiredService<IRestaurantSeeder>();
await seeder.SeedAsync();

// Configure the HTTP request pipeline.
app.UseSerilogRequestLogging();
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run();