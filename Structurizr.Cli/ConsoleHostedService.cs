using Microsoft.Extensions.Hosting;
using Structurizr;
using Microsoft.Extensions.Logging;
using System.Diagnostics;
using Structurizr.DslReader;
using structurizr_cli.Configuration;
using System.Net.Sockets;
using Ductus.FluentDocker.Services;
using Ductus.FluentDocker.Commands;
using Structurizr.Config;
using Structurizr.Util;
using System.Text;

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
          PushToStructurizr(generatedWorkspaceFileinfo);

        _hostHolder.Stop();
      }
      catch (Exception e)
      {
        _logger.LogError(e, e.Message);
        _hostHolder.Exit();
      }
    }

    private void PushToStructurizr(FileInfo generatedWorkspace)
    {
      //Powershell: docker run --rm -v ${PWD}/structurizr-gen:/usr/local/structurizr structurizr/cli push -url xxx-id xxx -key xxx -secret xxx -w ./workspace.dsl

      var command = $"docker run --rm - v {generatedWorkspace.DirectoryName}/:/usr/local/structurizr structurizr/cli push - url {_structurizrSettings.ApiUrl} -id {_structurizrSettings.WorkspaceId} -key {_structurizrSettings.ApiKey} -secret {_structurizrSettings.ApiSecret} -w ./workspace.dsl";
      _logger.LogInformation(command);
      var process = Process.Start("cmd.exe", $"/c {command}");

      process.WaitForExit();

      if(process.ExitCode != 0)
      {
        var sb = new StringBuilder();
        string? error;
        do
        {
          error = process.StandardError.ReadLine();
          sb.Append(error);
        } while (error != null);

        throw new Exception(sb.ToString());
      }
    }
  }
}