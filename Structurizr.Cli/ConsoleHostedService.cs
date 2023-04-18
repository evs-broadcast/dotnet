using Microsoft.Extensions.Hosting;
using Structurizr;
using Microsoft.Extensions.Logging;
using System.Diagnostics;
using Structurizr.DslReader;

namespace structurizr.Cli
{
  public sealed class ConsoleHostedService : BackgroundService
  {
    private readonly CliSettings _cliSettings;
    private readonly StructurizrSettings _structurizrSettings;
    private readonly HostHolder _hostHolder;
    private readonly ILogger<ConsoleHostedService> _logger;

    public ConsoleHostedService(CliSettings cliSettings, StructurizrSettings structurizrSettings,HostHolder hostHolder, ILogger<ConsoleHostedService> logger)
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

        if (_cliSettings.PushToStructurizr)
          Process.Start("cmd.exe", $"/c structurizr.bat push -url {_structurizrSettings.ApiUrl} -id {_structurizrSettings.WorkspaceId} -key {_structurizrSettings.ApiKey} -secret {_structurizrSettings.ApiSecret} -workspace {generatedWorkspaceFileinfo.FullName}");

        _hostHolder.Stop();        
      }
      catch (Exception e)
      {
        _logger.LogError(e.Message,e);
        _hostHolder.Exit();
      }
    }
  }
}