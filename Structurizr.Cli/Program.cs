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
);

var hostHolder = new HostHolder();

builder.ConfigureServices((hostContext, services) =>
   {
     var structurizrSettings = hostContext.Configuration.GetSection("Structurizr").Get<StructurizrSettings>() ?? new StructurizrSettings();
     structurizrSettings.ApiKey = hostContext.Configuration["Structurizr__ApiKey"];
     structurizrSettings.ApiSecret = hostContext.Configuration["Structurizr__ApiSecret"];

     var cliSettings = hostContext.Configuration.GetSection("Cli").Get<CliSettings>()??new CliSettings();

     services
     .AddLogging(b => b.AddConsole())
     .AddHostedService<ConsoleHostedService>()
     .AddSingleton(structurizrSettings)
     .AddSingleton(cliSettings)
     .AddSingleton(hostHolder);
   });

var host = builder.Build();
hostHolder.RegisterHost(host);
await host.StartAsync();