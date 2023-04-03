using Structurizr.DslReader.Parser;

namespace Structurizr.DslReader
{
    public sealed class DslFileReader
  {
    public async Task<ContextualWorkspace> ParseAsync(FileInfo fileInfo)
    {
      ArgumentNullException.ThrowIfNull(fileInfo, nameof(fileInfo));
      ArgumentNullException.ThrowIfNull(fileInfo.Directory, nameof(fileInfo.Directory));

      var streamReader = File.OpenText(fileInfo.FullName);
      var parsers = new IParser[] { new WorkspaceParser(), new EmptyLineParser(), new ModelParser(), new SoftwareSystemParser() };
      ContextualWorkspace contextualWorkspace = new ContextualWorkspace(null);

      if (streamReader is not null)
      {
        var line = await streamReader.ReadLineAsync();
        while (line != null)
        {
          try
          {
            var parser = parsers.FirstOrDefault(p => p.Accept(line,contextualWorkspace.Context));
            if (parser == null)
              throw new Exception("Unable to find Parser");
            contextualWorkspace = await parser.ParseAsync(Sanitize(line), contextualWorkspace, fileInfo.Directory);
            line = await streamReader.ReadLineAsync();
          }
          catch (Exception ex)
          {
            Console.WriteLine($"unable to parse {line}");
            Console.WriteLine(ex.Message);
          }
        }

        return contextualWorkspace;
      }
      else
      {
        throw new Exception($"Unable to open the file {fileInfo}");
      }
    }

    private static string Sanitize(string line)
    {
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