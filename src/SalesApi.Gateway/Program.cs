using Ocelot.DependencyInjection;
using Ocelot.Middleware;

var builder = WebApplication.CreateBuilder(args);

var config = builder.Configuration;

config.AddJsonFile("appsettings.json", true, true)
                    .AddJsonFile("ocelot.json");

var sv = builder.Services;
sv.AddOcelot(config);

var app = builder.Build();

app.UseHttpsRedirection();
app.UseOcelot().Wait();

app.MapGet("/", () =>
{
    return "Api Gateway Funcionando";
});

app.Run();