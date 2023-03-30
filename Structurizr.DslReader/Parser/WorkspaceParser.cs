using Structurizr;

namespace Structurizr.DslReader.Parser
{
  public sealed class WorkspaceParser : IParser
  {
    private const string WORKSPACE = "workspace";

    public bool Accept(string line)
    {
      return line.StartsWith(WORKSPACE, StringComparison.InvariantCultureIgnoreCase);
    }

    public async ValueTask<Workspace?> ParseAsync(string line, Workspace? workspace, DirectoryInfo directoryInfo)
    {
      if (workspace != null)
        throw new Exception("Unable to parse multiple workspace");

      var tokens = line.Split(" ").ToList();


      if (string.Compare(tokens[1], "extends", true) == 0)
      {
        var fileInfo = new FileInfo(Path.Combine(directoryInfo.FullName, tokens[2]));
        if (fileInfo.Exists)
        {
          return await new DslFileReader().ParseAsync(fileInfo);
        }
      }
      else
      {
        if (tokens.Count < 3)
          tokens.Add(string.Empty);
        workspace = new Workspace(tokens[1], tokens[2]);
      }

      return workspace;
    }
  }
}