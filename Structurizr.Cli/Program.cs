using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using structurizr.Cli;
using System.Reflection;

var builder = Host.CreateDefaultBuilder(args);

builder.ConfigureAppConfiguration(options =>
  options
  .AddJsonFile("appsettings.json", optional: true, reloadOnChange: true)
  .AddUserSecrets(Assembly.GetExecutingAssembly(), true)
  .AddEnvironmentVariables()
);

var hostHolder = new HostHolder();

builder.ConfigureServices((hostContext, services) =>
   {
     var structurizrSettings = hostContext.Configuration.GetSection("Structurizr").Get<StructurizrSettings>() ?? new StructurizrSettings();     

     var cliSettings = hostContext.Configuration.GetSection("Cli").Get<CliSettings>()??new CliSettings();

     services
     .AddLogging(b => b.AddConsole())
     .AddHostedService<ConsoleHostedService>()
     .AddSingleton(structurizrSettings)
     .AddSingleton(cliSettings)
     .AddSingleton(hostHolder);
   });

await builder.RunConsoleAsync(hostHolder.CancellationToken);