using Microsoft.Extensions.Hosting;
using Structurizr;
using Microsoft.Extensions.Logging;
using System.Diagnostics;
using Structurizr.DslReader;
using structurizr_cli.Configuration;
using System.Net.Sockets;
using Ductus.FluentDocker.Services;
using Ductus.FluentDocker.Commands;

namespace structurizr.Cli
{
  public sealed class ConsoleHostedService : BackgroundService
  {
    private readonly CliConfiguration _cliSettings;
    private readonly StructurizrConfiguration _structurizrSettings;
    private readonly HostHolder _hostHolder;
    private readonly ILogger<ConsoleHostedService> _logger;

    public ConsoleHostedService(CliConfiguration cliSettings, StructurizrConfiguration structurizrSettings, HostHolder hostHolder, ILogger<ConsoleHostedService> logger)
    {
      _cliSettings = cliSettings;
      _structurizrSettings = structurizrSettings;
      _hostHolder = hostHolder;
      _logger = logger;
    }

    protected override async Task ExecuteAsync(CancellationToken stoppingToken)
    {
      try
      {
        if (_cliSettings.BasePath == null)
          throw new Exception("Unable to merge workspace, base path not initialized.");

        _logger.LogInformation("Merging All workspace");

        Workspace? workspace = null;
        var directoryInfo = new DirectoryInfo(_cliSettings.BasePath);

        _logger.LogInformation($"Searching workspace.dsl files at {directoryInfo.FullName}");
        foreach (var fileInfo in directoryInfo.GetFiles("workspace.dsl", SearchOption.AllDirectories))
        {
          _logger.LogInformation($"Merging workspace from file {fileInfo.FullName}");
          workspace = await DslFileReader.ParseAsync(fileInfo, workspace, _logger);
        }

        if (workspace == null) throw new Exception("Unable to generate workspace");

        _logger.LogInformation($"Parsing succesfull software sytem:{workspace.Model.SoftwareSystems.Count}");

        _logger.LogInformation("retrieving ipd-via bundle from bitbucket");

        var generatedWorkspaceFileinfo = DslFileWriter.Write(workspace, directoryInfo);
        _logger.LogInformation($"Merged workspace in {generatedWorkspaceFileinfo.FullName}");

        if (_cliSettings.PushToStructurizr)
          //PushToStructurizr(generatedWorkspaceFileinfo.FullName);
          Process.Start("cmd.exe", $"/c structurizr.bat push -url {_structurizrSettings.ApiUrl} -id {_structurizrSettings.WorkspaceId} -key {_structurizrSettings.ApiKey} -secret {_structurizrSettings.ApiSecret} -workspace {generatedWorkspaceFileinfo.FullName}");

        _hostHolder.Stop();
      }
      catch (Exception e)
      {
        _logger.LogError(e, e.Message);
        _hostHolder.Exit();
      }
    }

    private void PushToStructurizr(string workspaceFullPath)
    {
      var hosts = new Hosts().Discover();
      var _docker = hosts.FirstOrDefault(x => x.IsNative) ?? hosts.FirstOrDefault(x => x.Name == "default");
      if (_docker == null)
        throw new Exception("No docker host running unable to push to structurizr");

      //var command = $"structurizr/cli push -url {_structurizrSettings.ApiUrl} -id {_structurizrSettings.WorkspaceId} -key {_structurizrSettings.ApiKey} -secret {_structurizrSettings.ApiSecret} -workspace {workspaceFullPath}";
      var command = $"structurizr/cli push -url {_structurizrSettings.ApiUrl} -id {_structurizrSettings.WorkspaceId} -key {_structurizrSettings.ApiKey} -secret {_structurizrSettings.ApiSecret}";
      _logger.LogInformation(command);
      var commandResponse = _docker.Host.Run(command,
        new Ductus.FluentDocker.Model.Containers.ContainerCreateParams
        {
          Interactive = true,
          //AutoRemoveContainer = true,
          Environment = new[] { $"PATH={workspaceFullPath}" }
        });
      if (!commandResponse.Success)
        throw new Exception(commandResponse.ToString());

      using var logs = _docker.Host.Logs(commandResponse.Data);

      while (!logs.IsFinished)
      {
        var line = logs.TryRead(5000); // Do a read with timeout
        if (line == null)
        {
          break;
        }

        _logger.LogInformation(line);
      }
    }
  }
}