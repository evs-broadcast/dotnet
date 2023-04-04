namespace Structurizr.DslReader.Parser
{
  public sealed class WorkspaceParser : IParser
  {
    private const string WORKSPACE = "workspace";

    public bool Accept(string line, ParsingContext context)
    {
      return line.StartsWith(WORKSPACE, StringComparison.InvariantCultureIgnoreCase);
    }

    public async ValueTask<ContextualWorkspace> ParseAsync(string line, ContextualWorkspace contextualWorkspace, DirectoryInfo directoryInfo)
    {
      ArgumentNullException.ThrowIfNull(contextualWorkspace, nameof(contextualWorkspace));

      var tokens = line.Split(" ").ToList();


      if (string.Compare(tokens[1], "extends", true) == 0)
      {
        var fileInfo = new FileInfo(Path.Combine(directoryInfo.FullName, tokens[2]));
        if (fileInfo.Exists)
        {
          contextualWorkspace = new ContextualWorkspace(await new DslFileReader().ParseAsync(fileInfo));
        }
      }
      else
      {
        if (tokens.Count < 3)
          tokens.Add(string.Empty);
        contextualWorkspace = new ContextualWorkspace(new Workspace(tokens[1], tokens[2]));
      }

      return contextualWorkspace;
    }
  }
}