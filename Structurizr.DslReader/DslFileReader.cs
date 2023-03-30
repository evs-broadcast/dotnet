using Structurizr.DslReader.Parser;

namespace Structurizr.DslReader
{
  public sealed class DslFileReader
  {
    public async Task<Workspace?> ParseAsync(FileInfo fileInfo)
    {
      var streamReader = File.OpenText(fileInfo.FullName);
      var parsers = new IParser[] { new WorkspaceParser(), new EmptyLineParser(), new ModelParser(), new SoftwareSystemParser() };
      Workspace? workspace = null;

      if (streamReader is not null)
      {
        var line = await streamReader.ReadLineAsync();
        while (line != null)
        {
          try
          {
            var parser = parsers.FirstOrDefault(p => p.Accept(line));
            if (parser == null)
              throw new Exception("Unable to find Parser");
            workspace = await parser.ParseAsync(Sanitize(line), workspace, fileInfo.Directory);
            line = await streamReader.ReadLineAsync();
          }
          catch (Exception ex)
          {
            Console.WriteLine($"unable to parse {line}");
            Console.WriteLine(ex.Message);
            return null;
          }
        }

        return workspace;
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