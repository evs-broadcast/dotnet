using Microsoft.Extensions.Logging;
using Structurizr.DslReader.Parser;

namespace Structurizr.DslReader
{
  public sealed class DslFileReader
  {
    public static async Task<Workspace> ParseAsync(FileInfo fileInfo, Workspace? workspace, ILogger logger)
    {
      ArgumentNullException.ThrowIfNull(fileInfo, nameof(fileInfo));
      ArgumentNullException.ThrowIfNull(fileInfo.Directory, nameof(fileInfo.Directory));

      var streamReader = File.OpenText(fileInfo.FullName);
      var parsers = ParserFactory.GetAllParsers();
      ContextualWorkspace? contextualWorkspace = new(workspace);

      if (streamReader is not null)
      {
        var line = Sanitize(await streamReader.ReadLineAsync());
        var lineNumber = 1;
        while (line != null)
        {
          if (!string.IsNullOrEmpty(line))
          {
            try
            {
              logger.LogDebug(line);
              var parser = parsers.FirstOrDefault(p => p.Accept(line, contextualWorkspace.Context));
              if (parser != null)
                contextualWorkspace = await parser.ParseAsync(line, contextualWorkspace, fileInfo.Directory, logger);
              else
                logger.LogInformation($"Unable to find parser for line:{line}");
            }
            catch (Exception ex)
            {
              logger.LogError($"[ERROR]unable to parse [{lineNumber} -> {line} in file:{fileInfo.Name}]");
              logger.LogError($"[ERROR]{ex.Message}");
              throw;
            }
          }
          line = Sanitize(await streamReader.ReadLineAsync());
          lineNumber++;
        }

        return contextualWorkspace.Workspace;
      }
      else
      {
        throw new Exception($"Unable to open the file {fileInfo}");
      }
    }

    private static string? Sanitize(string? line)
    {
      if (line == null)
        return null;

      line = line.Trim();
      int size;
      do
      {
        size = line.Length;
        line = line.Replace("  ", " ");
      } while (size > line.Length);

      return line;
    }
  }
}