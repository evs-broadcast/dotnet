using Structurizr.DslReader.Parser;

namespace Structurizr.DslReader
{
  public sealed class DslFileReader
  {
    public static async Task<Workspace> ParseAsync(FileInfo fileInfo, Workspace? workspace)
    {
      ArgumentNullException.ThrowIfNull(fileInfo, nameof(fileInfo));
      ArgumentNullException.ThrowIfNull(fileInfo.Directory, nameof(fileInfo.Directory));

      var streamReader = File.OpenText(fileInfo.FullName);
      var parsers = ParserFactory.GetAllParsers();
      ContextualWorkspace? contextualWorkspace = new(workspace);

      if (streamReader is not null)
      {
        var line = Sanitize(await streamReader.ReadLineAsync());
        while (line != null)
        {
          if (!string.IsNullOrEmpty(line))
          {
            try
            {
              Console.WriteLine(line);
              var parser = parsers.FirstOrDefault(p => p.Accept(line, contextualWorkspace.Context)) ?? throw new Exception($"Unable to find Parser for [{line}]");
              contextualWorkspace = await parser.ParseAsync(line, contextualWorkspace, fileInfo.Directory);
            }
            catch (Exception ex)
            {
              Console.WriteLine($"[ERROR]unable to parse [{line}]");
              Console.WriteLine($"[ERROR]{ex.Message}");
            }
          }
          line = Sanitize(await streamReader.ReadLineAsync());
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