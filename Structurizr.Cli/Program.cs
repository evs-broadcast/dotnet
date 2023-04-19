using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using structurizr.Cli;
using structurizr_cli.Configuration;
using System.Reflection;

var builder = Host.CreateDefaultBuilder(args);

builder.ConfigureAppConfiguration(options =>
  options
  .AddJsonFile("appsettings.json", optional: true, reloadOnChange: true)
  .AddUserSecrets(Assembly.GetExecutingAssembly(), true)
  .AddEnvironmentVariables()
  .AddCommandLine(args)
);

var hostHolder = new HostHolder();

builder.ConfigureServices((hostContext, services) =>
   {
     var structurizrSettings = hostContext.Configuration.GetSection("Structurizr").Get<StructurizrConfiguration>() ?? new StructurizrConfiguration();

     var cliSettings = hostContext.Configuration.Get<CliConfiguration>()??new CliConfiguration();

     services
     .AddLogging(b => b.AddConsole())
     .AddHostedService<ConsoleHostedService>()
     .AddSingleton(structurizrSettings)
     .AddSingleton(cliSettings)
     .AddSingleton(hostHolder);
   });

await builder.RunConsoleAsync(hostHolder.CancellationToken);